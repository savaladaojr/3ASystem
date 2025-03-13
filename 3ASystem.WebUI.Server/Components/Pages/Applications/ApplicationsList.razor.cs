using _3ASystem.Application.Applications.Queries.GetApplications;
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
		public NavigationManager Navigation { get; set; }


		private List<ApplicationResponse>? _records = null;
		private string _error = string.Empty;

		private Guid deleteId = Guid.Empty;



		protected override async Task OnInitializedAsync()
		{
			// Send an event to MediatR
			var result = await Mediator.Send(new GetApplicationsQuery());
			if (result.IsSuccess)
			{
				_records = result.Value;
			}
			else
			{
			}

		}
		private void CreateApplication()
		{
			Navigation.NavigateTo("/applications/create");
		}

		private void EditApplication(Guid id)
		{
			Navigation.NavigateTo("/applications/edit/" + id);
		}

		private void ShowCreateModal()
		{
			JSRuntime.InvokeVoidAsync("bootstrap.Modal.getOrCreateInstance", "#createApplicationModal").AsTask().Wait();
			JSRuntime.InvokeVoidAsync("bootstrap.Modal.show", "#createApplicationModal").AsTask().Wait();
		}

		private void ShowDeleteModal()
		{
			JSRuntime.InvokeVoidAsync("bootstrap.Modal.getOrCreateInstance", "#deleteConfirmationModal").AsTask().Wait();
			JSRuntime.InvokeVoidAsync("bootstrap.Modal.show", "#deleteConfirmationModal");
		}

		private void HideDeleteModal()
		{
			JSRuntime.InvokeVoidAsync("bootstrap.Modal.hide", "#deleteConfirmationModal");
		}

		private void ConfirmDelete(Guid id)
		{
			deleteId = id;
			ShowDeleteModal();
		}


		private void DeleteConfirmed()
		{
			if (deleteId == Guid.Empty) return;

			// Implement delete functionality here
			deleteId = Guid.Empty;
			HideDeleteModal();
		}

	}

}
