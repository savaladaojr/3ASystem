using _3ASystem.Application.UseCases.Applications.Queries.GetApplications;
using _3ASystem.Application.UseCases.Applications.Responses;
using _3ASystem.Application.UseCases.Functionalities.Commands.DeleteFunctionality;
using _3ASystem.Application.UseCases.Functionalities.Commands.EnableDisableFunctionality;
using _3ASystem.Application.UseCases.Functionalities.Queries.GetFunctionalitiesPaged;
using _3ASystem.Application.UseCases.Functionalities.Responses;
using _3ASystem.Application.UseCases.Modules.Commands.DeleteModule;
using _3ASystem.Application.UseCases.Modules.Commands.EnableDisableModule;
using _3ASystem.Application.UseCases.Modules.Queries.GetModuleById;
using _3ASystem.Application.UseCases.Modules.Queries.GetModulesPaged;
using _3ASystem.Application.UseCases.Modules.Responses;
using _3ASystem.WebUI.Server.Components._Shared;
using _3ASystem.WebUI.Server.Components.Pages.Modules;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace _3ASystem.WebUI.Server.Components.Pages.Functionalities
{
    public partial class FunctionalitiesList
	{
		[Inject]
		public IMediator Mediator { get; set; } = default!;

		[Inject]
		public IJSRuntime JSRuntime { get; set; } = default!;

		[Inject]
		public NavigationManager Navigation { get; set; } = default!;

		[Inject]
		public IDialogService DialogService { get; set; } = default!;

		[Inject]
		public ISnackbar Snackbar { get; set; } = default!;

		//filter options
		private Guid _filterApplicationId = Guid.Empty;
		private Guid _filterModuleId = Guid.Empty;

		private List<ApplicationResponse>? _applications = new List<ApplicationResponse>();
		private List<ModuleResponse>? _modules = new List<ModuleResponse>();

		//all records
		private List<FunctionalityResponse>? _records = [];

		private string _error = string.Empty;
		private bool isLoading = true;

		private int _totalOfRecords = 0;
		private int _selectedPage = 1;
		private int _pageSize = 10;
		private int _totalOfPages = 1;

		private IDialogReference dlg = default!;

		protected override async Task OnInitializedAsync()
		{
			await LoadApplications();
			await FetchData();
		}

		private async Task LoadApplications()
		{
			// Send an event to MediatR
			var result = await Mediator.Send(new GetApplicationsQuery());
			if (result.IsSuccess)
			{
				_applications = result.Value!;
			}
		}

		private async Task LoadApplicationModules(Guid applicationId)
		{
			// Send an event to MediatR
			var result = await Mediator.Send(new GetModulesByApplicationIdQuery() { AppId = applicationId });
			if (result.IsSuccess)
			{
				_modules = result.Value!;
			}
		}

		private async Task FetchData()
		{
			// Send an event to MediatR
			isLoading = true;
			var result = await Mediator.Send(new GetFunctionalitiesPagedQuery() { Page = _selectedPage, PageSize = _pageSize });
			if (result.IsSuccess)
			{
				_totalOfRecords = result.Value.TotalOfRecords;
				_totalOfPages = result.Value.TotalOfPages;

				_records = result.Value.Records;
			}
			else
			{
				_error = result.Error.Description;
			}
			isLoading = false;

		}

		private async Task PageChanged(int pageNumber)
		{
			_selectedPage = pageNumber;
			await FetchData();
			StateHasChanged();
		}

		private async Task CreateModule()
		{
			var parameters = new DialogParameters
			{
				{ nameof(ModalComponent.Icon), Icons.Material.Filled.AppRegistration},
				{ nameof(ModalComponent.Title), "Create New Functionality" },
				{ nameof(ModalComponent.Body), true },
				{ nameof(ModalComponent.SubmitText), "Save"},
				{ nameof(ModalComponent.ShowActionButtons), false }

				/*{ nameof(ModalComponent.OnSubmit), EventCallback.Factory.Create(this, () => {
					Console.WriteLine(3);
					DialogService.Close(dlg, DialogResult.Cancel());
				})}*/
			};

			parameters.Add(nameof(ModalComponent.Body), (RenderFragment)(builder =>
			{
				builder.OpenComponent<FunctionalityCreateForm>(0);
				builder.AddComponentParameter(1,
					nameof(ModuleCreateForm.OnSaveClickSuccess),
					EventCallback.Factory.Create(this, async (string successMessage) =>
					{
						DialogService.Close(dlg, DialogResult.Cancel());

						Snackbar.Clear();
						Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
						Snackbar.Add(successMessage, Severity.Success);

						await FetchData();
						StateHasChanged();
					})
				);

				builder.AddComponentParameter(2,
					nameof(ModuleCreateForm.OnCancelClick),
					EventCallback.Factory.Create(this, () =>
					{
						DialogService.Close(dlg, DialogResult.Cancel());
					})
				);

				builder.CloseComponent();
			}));

			var options = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true, CloseButton = false, CloseOnEscapeKey = false, BackdropClick = false };
			dlg = await DialogService.ShowAsync<ModalComponent>("Create New Functionality", parameters, options);

		}


		private async Task EditModule(Guid id)
		{

			var parameters = new DialogParameters
			{
				{ nameof(ModalComponent.Icon), Icons.Material.Filled.AppRegistration},
				{ nameof(ModalComponent.Title), "Edit Functionality" },
				{ nameof(ModalComponent.Body), true },
				{ nameof(ModalComponent.SubmitText), "Save"},
				{ nameof(ModalComponent.ShowActionButtons), false }

				/*{ nameof(ModalComponent.OnSubmit), EventCallback.Factory.Create(this, () => {
					Console.WriteLine(3);
					DialogService.Close(dlg, DialogResult.Cancel());
				})}*/
			};

			parameters.Add(nameof(ModalComponent.Body), (RenderFragment)(builder =>
			{
				builder.OpenComponent<FunctionalityUpdateForm>(0);
							
				builder.AddComponentParameter(2, nameof(FunctionalityUpdateForm.Id), id);

				builder.AddComponentParameter(3,
					nameof(FunctionalityUpdateForm.OnSaveClickSuccess),
					EventCallback.Factory.Create(this, async (string successMessage) =>
					{
						DialogService.Close(dlg, DialogResult.Cancel());

						Snackbar.Clear();
						Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
						Snackbar.Add(successMessage, Severity.Success);

						await FetchData();
						StateHasChanged();

					})
				);

				builder.AddComponentParameter(4,
					nameof(FunctionalityUpdateForm.OnCancelClick),
					EventCallback.Factory.Create(this, () =>
					{
						DialogService.Close(dlg, DialogResult.Cancel());
					})
				);

				builder.CloseComponent();
			}));

			var options = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true, CloseButton = false, CloseOnEscapeKey = false, BackdropClick = false };
			dlg = await DialogService.ShowAsync<ModalComponent>("Edit Functionality", parameters, options);
		}


		private async Task AskConfirmDelete(Guid id)
		{
			var parameters = new DialogParameters<ConfirmDialogComponent>
			{
				{ x => x.ContentText, "Do you really want to delete this record? This process cannot be undone." },
				{ x => x.ButtonText, "Delete" },
				{ x => x.Color, Color.Error }
			};

			var options = new DialogOptions() { MaxWidth = MaxWidth.ExtraSmall, CloseButton = true, CloseOnEscapeKey = true, BackdropClick = false };

			var dialog = await DialogService.ShowAsync<ConfirmDialogComponent>("Delete", parameters, options);
			var result = await dialog.Result;
			if (result is not null && result.Canceled is false)
			{
				var data = (bool)result.Data!;
				if (data)
				{
					await ConfirmDelete(id);
				}
			}
		}

		private async Task ConfirmDelete(Guid id)
		{
			if (id == Guid.Empty) return;

			// Send an event to MediatR
			var result = await Mediator.Send(new DeleteFunctionalityCommand(id));
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

		private async void EnableDisable(Guid id)
		{
			var result = await Mediator.Send(new EnableDisableFunctionalityCommand() { Id = id});
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
