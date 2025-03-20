using _3ASystem.Application.Applications.Commands.DeleteApplication;
using _3ASystem.Application.Applications.Commands.EnableDisableApplication;
using _3ASystem.Application.Applications.Queries.GetApplications;
using _3ASystem.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace _3ASystem.WebUI.Server.Components.Pages.Applications
{
    public partial class ApplicationsList 
	{
		[Inject]
		public IMediator Mediator { get; set; } = default!;

		[Inject]
		public IJSRuntime JSRuntime { get; set; } = default!;

		[Inject]
		public NavigationManager Navigation { get; set; } = default!;


		private ApplicationCreateForm applicationCreateForm = default!;
		private ApplicationUpdateForm applicationUpdateForm = default!;


		private List<ApplicationResponse>? _records = null;
		private string _error = string.Empty;

		private Guid deleteId = Guid.Empty;



		protected override async Task OnInitializedAsync()
		{
			await FetchData();

		}

		private async Task FetchData()
		{
			// Send an event to MediatR
			var result = await Mediator.Send(new GetApplicationsQuery());
			if (result.IsSuccess)
			{
				_records = result.Value;
			}
			else
			{
				_error = result.Error.Description;
			}
		}

		private async Task CreateApplication()
		{
			await applicationCreateForm.Start();
			await ShowCreateModal();
			//Navigation.NavigateTo("/applications/create");
		}

		private async Task ShowCreateModal()
		{
			//JSRuntime.InvokeVoidAsync("bootstrap.Modal.getOrCreateInstance", "#createApplicationModal").AsTask().Wait();
			//JSRuntime.InvokeVoidAsync("bootstrap.Modal.show", "#createApplicationModal").AsTask().Wait();

			// Using the Bootstrap 5 Modal API with options to disable closing when clicking outside or pressing ESC
			await JSRuntime.InvokeVoidAsync("eval", "new bootstrap.Modal(document.getElementById('createApplicationModal'), { backdrop: 'static', keyboard: true }).show();");
		}

		private async Task HideModalCreateModal()
		{
			//JSRuntime.InvokeVoidAsync("bootstrap.Modal.hide", "#deleteConfirmationModal");
			await JSRuntime.InvokeVoidAsync("eval", "bootstrap.Modal.getOrCreateInstance(document.getElementById('createApplicationModal')).hide()");
		}

		private async Task ApplicationCreateForm_CancelClick()
		{
			await HideModalCreateModal();
		}

		private async Task ApplicationCreateForm_SaveClickSuccess()
		{
			await HideModalCreateModal();
			await FetchData();
			StateHasChanged();
		}



		private async Task EditApplication(Guid id)
		{

			await applicationUpdateForm.Start(id);
			await ShowUpdateModal();
			//Navigation.NavigateTo("/applications/edit/" + id);
		}

		private async Task ShowUpdateModal()
		{
			//JSRuntime.InvokeVoidAsync("bootstrap.Modal.getOrCreateInstance", "#createApplicationModal").AsTask().Wait();
			//JSRuntime.InvokeVoidAsync("bootstrap.Modal.show", "#createApplicationModal").AsTask().Wait();

			// Using the Bootstrap 5 Modal API with options to disable closing when clicking outside or pressing ESC
			await JSRuntime.InvokeVoidAsync("eval", "new bootstrap.Modal(document.getElementById('updateApplicationModal'), { backdrop: 'static', keyboard: true }).show();");
		}

		private async Task HideModalUpdateModal()
		{
			//JSRuntime.InvokeVoidAsync("bootstrap.Modal.hide", "#deleteConfirmationModal");
			await JSRuntime.InvokeVoidAsync("eval", "bootstrap.Modal.getOrCreateInstance(document.getElementById('updateApplicationModal')).hide()");
		}

		private async Task ApplicationUpdateForm_CancelClick()
		{
			await HideModalUpdateModal();
		}

		private async Task ApplicationUpdateForm_SaveClickSuccess()
		{
			await HideModalUpdateModal();
			await FetchData();
			StateHasChanged();
		}




		private async Task AskConfirmDelete(Guid id)
		{
			deleteId = id;
			await ShowModalAskConfirm();
		}

		private async Task ShowModalAskConfirm()
		{
			//JSRuntime.InvokeVoidAsync("bootstrap.Modal.getOrCreateInstance", "#deleteConfirmationModal").AsTask().Wait();
			//JSRuntime.InvokeVoidAsync("bootstrap.Modal.show", "#deleteConfirmationModal");
			await JSRuntime.InvokeVoidAsync("eval", "new bootstrap.Modal(document.getElementById('deleteConfirmationModal'), { backdrop: 'static', keyboard: true }).show();");
		}

		private async Task ConfirmDelete()
		{
			if (deleteId == Guid.Empty) return;

			// Send an event to MediatR
			var result = await Mediator.Send(new DeleteApplicationsCommand(deleteId));
			if (result.IsSuccess)
			{
				await FetchData();
				StateHasChanged();
			}
			else
			{
				_error = result.Error.Description;
			}

			deleteId = Guid.Empty;
			await HideModalAskConfirmDelete();
		}

		private async Task HideModalAskConfirmDelete()
		{
			//JSRuntime.InvokeVoidAsync("bootstrap.Modal.hide", "#deleteConfirmationModal");
			await JSRuntime.InvokeVoidAsync("eval", "bootstrap.Modal.getOrCreateInstance(document.getElementById('deleteConfirmationModal')).hide()");
		}


		private async void EnableDisable(Guid id)
		{
			var result = await Mediator.Send(new EnableDisableApplicationCommand() { Id = id});
			if (result.IsSuccess)
			{
				await FetchData();
				StateHasChanged();
			}
			else
			{
				_error = result.Error.Description;
			}

		}

	}

}
