using _3ASystem.Application.UseCases.Applications.Queries.GetApplications;
using _3ASystem.Application.UseCases.Applications.Responses;
using _3ASystem.Application.UseCases.Functionalities.Commands.UpdateFunctionality;
using _3ASystem.Application.UseCases.Functionalities.Queries.GetFunctionalityById;
using _3ASystem.Application.UseCases.Modules.Queries.GetModuleById;
using _3ASystem.Application.UseCases.Modules.Responses;
using _3ASystem.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace _3ASystem.WebUI.Server.Components.Pages.Functionalities
{
	public partial class FunctionalityUpdateForm : ComponentBase
	{
        // Define an EventCallback to notify the parent component
        [Parameter] public EventCallback<string> OnSaveClickSuccess { get; set; }
        [Parameter] public EventCallback OnCancelClick { get; set; }

		[Inject]
		public IMediator Mediator { get; set; } = default!;

		[Inject]
		public IJSRuntime JSRuntime { get; set; } = default!;

		[Inject]
		public NavigationManager Navigation { get; set; } = default!;

		[Parameter]
		public Guid Id { get; set; }


        public List<ApplicationResponse> ListOfApplications { get; set; } = [];
        public List<ModuleResponse> ListOfModules { get; set; } = [];

        private UpdateFunctionalityCommand updateFunctionality = default!;

		private string _error = string.Empty;
		private bool _isSubmitting = false;

		private EditContext? _editContext = default!;
		private ValidationMessageStore? _messageStore = default!;

		protected override async Task OnInitializedAsync()
		{
			await Start();
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
                    await LoadApplicationModules(result.Value.ApplicationId);

                    updateFunctionality = new UpdateFunctionalityCommand
					{
						Id = result.Value.Id,
						ApplicationId = result.Value.ApplicationId,
                        ModuleId = result.Value.ModuleId,
						Name = result.Value.Name,
                        Description = result.Value.Description,
                        Abbreviation = result.Value.Abbreviation,
						Route = result.Value.Route,
                        IconUrl = result.Value.IconUrl,
						FriendlyId =  result.Value.FriendlyId,
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
			_messageStore!.Clear();

			var result = await Mediator.Send(updateFunctionality);

			if (result.IsSuccess)
			{
				// Call the parent method via the EventCallback
				await OnSaveClickSuccess.InvokeAsync($"Module [{result.Value.Name}] successfully updated.");
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
							_messageStore.Add(_editContext!.Field(validationFailure.PropertyName), validationFailure.ErrorMessage);
						}
					}

					_editContext!.NotifyValidationStateChanged();
				}
				_error = result.Error.Description;
				
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
			_editContext!.NotifyValidationStateChanged();
		}
	}

}