using _3ASystem.Application.Applications.Commands.CreateApplication;
using _3ASystem.Application.Applications.Commands.UpdateApplication;
using _3ASystem.Application.Applications.Queries.GetApplicationById;
using _3ASystem.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace _3ASystem.WebUI.Server.Components.Pages.Applications
{


	public partial class ApplicationCreateForm : ComponentBase
	{
		[Inject]
		public IMediator Mediator { get; set; } = default!;
		[Inject]
		public IJSRuntime JSRuntime { get; set; } = default!;

		[Inject]
		public NavigationManager Navigation { get; set; } = default!;


		private CreateApplicationCommand createApplication = new CreateApplicationCommand();

		private string _error = string.Empty;
		private bool _isSubmitting = false;

		private EditContext? _editContext = default!;
		private ValidationMessageStore? _messageStore = default!;

		protected override Task OnInitializedAsync()
		{
			_editContext = new EditContext(createApplication);
			_messageStore = new ValidationMessageStore(_editContext);

			return Task.CompletedTask;
		}


		private async Task HandleSubmit()
		{
			_isSubmitting = true;
			_error = string.Empty;
			var result = await Mediator.Send(createApplication);

			if (result.IsSuccess)
			{
				// Close the modal
				Navigation.NavigateTo("/applications");
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
				_error = result.Error.Description;

			}
			_isSubmitting = false;
		}

		private void ClearValidationMessage(string fieldName)
		{
			if (_messageStore is null) return;

			_messageStore.Clear(_editContext!.Field(fieldName));
			_editContext.NotifyValidationStateChanged();
		}

	}

}