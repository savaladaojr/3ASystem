using _3ASystem.Application.UseCases.Applications.Queries.GetApplications;
using _3ASystem.Application.UseCases.Applications.Responses;
using _3ASystem.Application.UseCases.Modules.Commands.DeleteModule;
using _3ASystem.Application.UseCases.Modules.Commands.EnableDisableModule;
using _3ASystem.Application.UseCases.Modules.Queries.GetModulesPaged;
using _3ASystem.Application.UseCases.Modules.Responses;
using _3ASystem.WebUI.Server.Components._Shared;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace _3ASystem.WebUI.Server.Components.Pages.Modules
{
    public partial class ModulesList 
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


		private List<ApplicationResponse>? _applications = new List<ApplicationResponse>();

		private List<ModuleResponse>? _records = [];

		private string _error = string.Empty;
		private bool isLoading = true;

		private int _totalOfRecords = 0;
		private int _selectedPage = 1;
		private int _pageSize = 10;
		private int _totalOfPages = 1;

		private IDialogReference dlg = default!;

		protected override async Task OnInitializedAsync()
		{
			await LoadApplication();
			await FetchData();
		}


		private async Task LoadApplication()
		{
			// Send an event to MediatR
			var result = await Mediator.Send(new GetApplicationsQuery());
			if (result.IsSuccess)
			{
				_applications = result.Value!;
			}

		}

		private async Task FetchData()
		{
			// Send an event to MediatR
			isLoading = true;
			var result = await Mediator.Send(new GetModulesPagedQuery() { Page = _selectedPage, PageSize = _pageSize });
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
				{ nameof(ModalComponent.Title), "Create New Module" },
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
				builder.OpenComponent<ModuleCreateForm>(0);
				builder.AddComponentParameter(1,
					nameof(ModuleCreateForm.ListOfApplications),
					_applications
				);

				builder.AddComponentParameter(2,
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

				builder.AddComponentParameter(3,
					nameof(ModuleCreateForm.OnCancelClick),
					EventCallback.Factory.Create(this, () =>
					{
						DialogService.Close(dlg, DialogResult.Cancel());
					})
				);

				builder.CloseComponent();
			}));

			var options = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true, CloseButton = false, CloseOnEscapeKey = false, BackdropClick = false };
			dlg = await DialogService.ShowAsync<ModalComponent>("Create New Module", parameters, options);

		}


		private async Task EditModule(Guid id)
		{

			var parameters = new DialogParameters
			{
				{ nameof(ModalComponent.Icon), Icons.Material.Filled.AppRegistration},
				{ nameof(ModalComponent.Title), "Edit Module" },
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
				builder.OpenComponent<ModuleUpdateForm>(0);
				
				builder.AddComponentParameter(1,
					nameof(ModuleUpdateForm.ListOfApplications),
					_applications
				);
				
				builder.AddComponentParameter(2, nameof(ModuleUpdateForm.Id), id);

				builder.AddComponentParameter(3,
					nameof(ModuleUpdateForm.OnSaveClickSuccess),
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
					nameof(ModuleUpdateForm.OnCancelClick),
					EventCallback.Factory.Create(this, () =>
					{
						DialogService.Close(dlg, DialogResult.Cancel());
					})
				);

				builder.CloseComponent();
			}));

			var options = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true, CloseButton = false, CloseOnEscapeKey = false, BackdropClick = false };
			dlg = await DialogService.ShowAsync<ModalComponent>("Edit Module", parameters, options);
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
			var result = await Mediator.Send(new DeleteModuleCommand(id));
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
			var result = await Mediator.Send(new EnableDisableModuleCommand() { Id = id});
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
