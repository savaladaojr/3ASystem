using _3ASystem.Application.UseCases.Applications.Queries.GetApplications;
using _3ASystem.Application.UseCases.Applications.Responses;
using _3ASystem.Application.UseCases.Functionalities.Commands.CreateFunctionality;
using _3ASystem.Application.UseCases.Modules.Queries.GetModuleById;
using _3ASystem.Application.UseCases.Modules.Responses;
using _3ASystem.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.ComponentModel.DataAnnotations;

namespace _3ASystem.WebUI.Server.Components.Pages.Functionalities
{
	public record CreateFunctionality
	{
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

	}

	public partial class FunctionalityCreateForm : ComponentBase
	{
		// Define an EventCallback to notify the parent component
		[Parameter] public EventCallback<string> OnSaveClickSuccess { get; set; }
		[Parameter] public EventCallback OnCancelClick { get; set; }

		public Guid ApplicationId { get; set; } = default!;

		public List<ApplicationResponse> ListOfApplications { get; set; } = [];
		public List<ModuleResponse> ListOfModules { get; set; } = [];

		[Inject]
		public IMediator Mediator { get; set; } = default!;

		[Inject]
		public IJSRuntime JSRuntime { get; set; } = default!;

		[Inject]
		public NavigationManager Navigation { get; set; } = default!;

		private CreateFunctionality createFunctionality = default!;

		private string _error = string.Empty;
		private bool _isSubmitting = false;

		private EditContext? _editContext = default!;
		private ValidationMessageStore? _messageStore = default!;

		protected override async Task OnInitializedAsync()
		{
			await Start();

			// Load the list of applications and modules
			await LoadApplications();

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
			createFunctionality = new CreateFunctionality();

			_editContext = new EditContext(createFunctionality);
			_messageStore = new ValidationMessageStore(_editContext);

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
			var command = new CreateFunctionalityCommand()
			{
				ModuleId = createFunctionality.ModuleId,
				Name = createFunctionality.Name,
				Abbreviation = createFunctionality.Abbreviation,
				Route = createFunctionality.Route,
				FriendlyId = createFunctionality.FriendlyId,
				IconUrl = createFunctionality.IconUrl,
				IsPartOfMenu = true 
			};
			var result = await Mediator.Send(command);

			if (result.IsSuccess)
			{
				// Call the parent method via the EventCallback
				await OnSaveClickSuccess.InvokeAsync($"Functionality [{result.Value.Name}] successfully created.");
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