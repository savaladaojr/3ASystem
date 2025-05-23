using _3ASystem.Application.UseCases.Applications.Commands.CreateApplication;
using _3ASystem.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace _3ASystem.WebUI.Server.Components.Pages.Applications
{


	public partial class ApplicationCreateForm : ComponentBase
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


		private CreateApplicationCommand createApplication = default!;

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
			createApplication = new CreateApplicationCommand();
			_editContext = new EditContext(createApplication);
			_messageStore = new ValidationMessageStore(_editContext);

			var task = await Task.FromResult(true);
			return task;
		}

		private async Task HandleCancelAsync()
		{
			// Call the parent method via the EventCallback
			await OnCancelClick.InvokeAsync(null);
		}

		private async Task HandleCreateAsync()
		{
			if (!IsValidSubmit()) return;

			_isSubmitting = true;
			_error = string.Empty;

			//Call the create command
			var result = await Mediator.Send(createApplication);

			if (result.IsSuccess)
			{
				// Call the parent method via the EventCallback
				await OnSaveClickSuccess.InvokeAsync($"Application [{result.Value.Name}] successfully created." );
			}
			else
			{
				if (result.Error.Type is ErrorType.Validation)
				{
					foreach (var item in ((ValidationError)result.Error).Errors)
					{
						if ((item.ErrorObject is not null) && (item.ErrorObject.GetType() == typeof(FluentValidation.Results.ValidationFailure)))
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