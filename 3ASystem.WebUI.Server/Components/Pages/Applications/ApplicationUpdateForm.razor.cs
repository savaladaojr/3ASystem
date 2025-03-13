using _3ASystem.Application.Applications.Commands.CreateApplication;
using _3ASystem.Application.Applications.Commands.UpdateApplication;
using _3ASystem.Application.Applications.Queries.GetApplicationById;
using _3ASystem.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

namespace _3ASystem.WebUI.Server.Components.Pages.Applications
{


	public partial class ApplicationUpdateForm : ComponentBase
	{
		[Inject]
		public IMediator Mediator { get; set; } = default!;

		[Inject]
		public IJSRuntime JSRuntime { get; set; } = default!;

		[Inject]
		public NavigationManager Navigation { get; set; } = default!;

		[Parameter]
		public Guid Id { get; set; }

		private UpdateApplicationCommand updateApplication = default!;

		private string _error = string.Empty;
		private bool _isSubmitting = false;

		private EditContext? _editContext = default!;
		private ValidationMessageStore? _messageStore = default!;

		protected override async Task OnInitializedAsync()
		{
			// Send an event to MediatR
			if (Id != Guid.Empty)
			{
				var result = await Mediator.Send(new GetApplicationByIdQuery() { Id = Id });
				if (result.IsSuccess)
				{
					updateApplication = new UpdateApplicationCommand
					{
						Id = result.Value.Id,
						Name = result.Value.Name,
						Abbreviation = result.Value.Abbreviation,
						Description = result.Value.Description,
						IconUrl = result.Value.IconUrl,
						IsActive = result.Value.IsActive
					};

					_editContext = new EditContext(updateApplication);
					_messageStore = new ValidationMessageStore(_editContext);
				}
				else
				{
					_error = result.Error.Description;
				}
			}
			else
			{
			}

		}

		private async Task HandleSubmit()
		{
			_isSubmitting = true;
			_error = string.Empty;
			_messageStore!.Clear();

			var result = await Mediator.Send(updateApplication);

			if (result.IsSuccess)
			{
				// Close the modal
				Navigation.NavigateTo("/applications");
			}
			else
			{
				if (result.Error.Type is Domain.Shared.ErrorType.Validation)
				{
					foreach (var item in ((ValidationError)result.Error).Errors)
					{
						if ((item.ErrorObject is not null) && (item.ErrorObject.GetType() == typeof(FluentValidation.Results.ValidationFailure)))
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

		private void CancelUpdate()
		{
			Navigation.NavigateTo("/applications");
		}

		private void ClearValidationMessage(string fieldName)
		{
			if (_messageStore is null) return;

			_messageStore.Clear(_editContext!.Field(fieldName));
			_editContext!.NotifyValidationStateChanged();
		}
	}

}