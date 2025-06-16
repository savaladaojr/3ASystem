using _3ASystem.Application.UseCases.Applications.Queries.GetApplications;
using _3ASystem.Application.UseCases.Applications.Responses;
using _3ASystem.Application.UseCases.Functionalities.Commands.CreateFunctionality;
using _3ASystem.Application.UseCases.Functionalities.Commands.UpdateFunctionality;
using _3ASystem.Application.UseCases.Functionalities.Queries.GetFunctionalityById;
using _3ASystem.Application.UseCases.Modules.Commands.UpdateModule;
using _3ASystem.Application.UseCases.Modules.Queries.GetModuleById;
using _3ASystem.Application.UseCases.Modules.Responses;
using _3ASystem.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.ComponentModel.DataAnnotations;

namespace _3ASystem.WebUI.Server.Components.Pages.Functionalities
{
	public record UpdateFunctionalityModel
	{
		[Key]
		public Guid Id { get; set; } = Guid.Empty;

		[Required(ErrorMessage = "Select an Application")]
		public Guid ApplicationId { get; set; } = Guid.Empty;

		[Required(ErrorMessage = "Select a Module")]
		public Guid ModuleId { get; set; } = Guid.Empty;

		[Required(ErrorMessage = "Name is required")]
		public string Name { get; set; } = string.Empty;

		[Required(ErrorMessage = "Abbreviation is required")]
		public string Abbreviation { get; set; } = string.Empty;

		[Required(ErrorMessage = "Route is required")]
		public string Route { get; set; } = string.Empty;
		public string IconUrl { get; set; } = string.Empty;

		[Required(ErrorMessage = "Friendly ID is required")]
		public string FriendlyId { get; set; } = string.Empty;

		public bool IsPartOfMenu { get; set; } = true;

		public bool IsActive { get; set; } = true;
	}

	public partial class FunctionalityUpdateForm : ComponentBase
	{
		// Define an EventCallback to notify the parent component
		[Parameter] public EventCallback<string> OnSaveClickSuccess { get; set; }
		[Parameter] public EventCallback OnCancelClick { get; set; }

		public List<ApplicationResponse> ListOfApplications { get; set; } = [];
		public List<ModuleResponse> ListOfModules { get; set; } = [];

		[Inject]
		public IMediator Mediator { get; set; } = default!;

		[Inject]
		public IJSRuntime JSRuntime { get; set; } = default!;

		[Inject]
		public NavigationManager Navigation { get; set; } = default!;

		[Parameter]
		public Guid Id { get; set; }

		private UpdateFunctionalityModel updateFunctionality = default!;

		private string _error = string.Empty;
		private bool _isSubmitting = false;

		private EditContext? _editContext = default!;
		private ValidationMessageStore? _messageStore = default!;

		protected override async Task OnInitializedAsync()
		{
			await Start();
		}


		private async Task LoadApplications()
		{
			// Send an event to MediatR
			var result = await Mediator.Send(new GetApplicationsQuery());
			if (result.IsSuccess)
			{
				ListOfApplications = result.Value;
			}

		}

		private async Task LoadApplicationModules(Guid applicationId)
		{
			// Send an event to MediatR
			var result = await Mediator.Send(new GetModulesByApplicationIdQuery() { AppId = applicationId });
			if (result.IsSuccess)
			{
				ListOfModules = result.Value;
			}
		}

		public async Task<bool> Start()
		{
			// Send an event to MediatR
			if (Id != Guid.Empty)
			{
				var result = await Mediator.Send(new GetFunctionalityByIdQuery() { Id = Id });
				if (result.IsSuccess)
				{
					// Load the list of applications and modules
					await LoadApplications();
					await LoadApplicationModules(result.Value.Module!.ApplicationId);

					updateFunctionality = new UpdateFunctionalityModel
					{
						Id = result.Value.Id,
						ApplicationId = result.Value.Module!.ApplicationId,
						ModuleId = result.Value.ModuleId,
						Name = result.Value.Name,
						Abbreviation = result.Value.Abbreviation,
						Route = result.Value.Route,
						FriendlyId = result.Value.FriendlyId,
						IconUrl = result.Value.IconUrl,
						IsPartOfMenu = result.Value.IsPartOfMenu,
						IsActive = result.Value.IsActive,
					};

					_editContext = new EditContext(updateFunctionality);
					_messageStore = new ValidationMessageStore(_editContext);
				}
				else
				{
					_error = result.Error.Description;
				}
			}

			var task = await Task.FromResult(true);
			return task;
		}

		private async Task HandleCancelAsync()
		{
			// Call the parent method via the EventCallback
			await OnCancelClick.InvokeAsync(null);
		}

		private async Task HandleSubmitAsync()
		{
			if (!IsValidSubmit()) return;

			_isSubmitting = true;
			_error = string.Empty;

			//Call the create command
			var command = new UpdateFunctionalityCommand()
			{
				Id = updateFunctionality.Id,
				ModuleId = updateFunctionality.ModuleId,
				Name = updateFunctionality.Name,
				Abbreviation = updateFunctionality.Abbreviation,
				Route = updateFunctionality.Route,
				FriendlyId = updateFunctionality.FriendlyId,
				IconUrl = updateFunctionality.IconUrl,
				IsPartOfMenu = updateFunctionality.IsPartOfMenu
			};
			var result = await Mediator.Send(command);

			if (result.IsSuccess)
			{
				// Call the parent method via the EventCallback
				await OnSaveClickSuccess.InvokeAsync($"Functionality [{result.Value.Name}] successfully update.");
			}
			else
			{
				if (result.Error.Type is ErrorType.Validation)
				{
					foreach (var item in ((ValidationError)result.Error).Errors)
					{
						if (item.ErrorObject is not null && item.ErrorObject.GetType() == typeof(FluentValidation.Results.ValidationFailure))
						{
							var validationFailure = (FluentValidation.Results.ValidationFailure)item.ErrorObject;
							_messageStore!.Add(_editContext!.Field(validationFailure.PropertyName), validationFailure.ErrorMessage);
						}
					}

					_editContext!.NotifyValidationStateChanged();
				}
				else
				{
					_error = result.Error.Description;
				}

			}
			_isSubmitting = false;
		}


		private bool IsValidSubmit()
		{
			var valid = _editContext!.Validate();
			return valid;
		}

		private void ClearValidationMessage(string fieldName)
		{
			if (_messageStore is null) return;

			_messageStore.Clear(_editContext!.Field(fieldName));
			_editContext.NotifyValidationStateChanged();
		}

	}

}