using _3ASystem.Application.UseCases.Operations.Commands.AddBasicOperations;
using _3ASystem.Application.UseCases.Operations.Commands.DeleteOperation;
using _3ASystem.Application.UseCases.Operations.Commands.EnableDisableOperation;
using _3ASystem.Application.UseCases.Operations.Queries.GetOperationsByFunctionality;
using _3ASystem.Application.UseCases.Operations.Responses;
using _3ASystem.Domain.Entities.Operations;
using _3ASystem.WebUI.Server.Components._Shared;

using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace _3ASystem.WebUI.Server.Components.Pages.Functionalities.Operations
{
    public partial class OperationsModal
    {
        [CascadingParameter]
        private IMudDialogInstance MudDialog { get; set; } = default!;

        [Parameter]
        public Guid FunctionalityId { get; set; }

        [Inject]
        public IMediator Mediator { get; set; } = default!;

        [Inject]
        public IDialogService DialogService { get; set; } = default!;

        [Inject]
        public ISnackbar Snackbar { get; set; } = default!;

        private List<OperationResponse> _operations = new();
        private bool isLoading = true;

        protected override async Task OnInitializedAsync()
        {
            await LoadOperations();
        }

        private async Task LoadOperations()
        {
            isLoading = true;
            var result = await Mediator.Send(new GetOperationsByFunctionalityQuery { FunctionalityId = FunctionalityId });
            if (result.IsSuccess)
            {
                _operations = result.Value ?? new();
            }
            isLoading = false;
        }

        private async Task OpenCreateOperationDialog()
        {
            var parameters = new DialogParameters
            {
                { nameof(OperationForm.IsEditMode), false },
                { nameof(OperationForm.FunctionalityId), FunctionalityId },
            };

            var dialog = await DialogService.ShowAsync<OperationForm>("Create New Operation", parameters);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                await LoadOperations();
                ShowSuccessMessage(result.Data as string ?? "Operation created successfully!");
            }
        }

        private async Task OpenEditOperationDialog(Guid id, string name, string code, string friendlyId)
        {
            var parameters = new DialogParameters
        {
            { nameof(OperationForm.IsEditMode), true },
            { nameof(OperationForm.FunctionalityId), FunctionalityId },
            { "OperationId", id },
            { "OperationName", name },
            { "OperationCode", code },
            { "OperationFriendlyId", friendlyId }
        };

            var dialog = await DialogService.ShowAsync<OperationForm>("Edit Operation", parameters);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                await LoadOperations();
                ShowSuccessMessage(result.Data as string ?? "Operation updated successfully!");
            }
        }

        private async Task AddBasicOperations()
        {
            var command = new  AddBasicOperationsCommand{ FunctionalityId = FunctionalityId};
            var result = await Mediator.Send(command);

            if (result.IsSuccess)
            {
                await LoadOperations();
                ShowSuccessMessage("Basic operations are available.");
            }
            else
            {
                ShowErrorMessage(result.Error.Description);
            }
        }

        private async Task ToggleActiveStatus(Guid operationId, bool isActive)
        {
            var command = new EnableDisableOperationCommand { Id = operationId, IsActive = isActive };
            var result = await Mediator.Send(command);

            if (result.IsSuccess)
            {
                await LoadOperations();
                string message = isActive ? "Operation enabled successfully!" : "Operation disabled successfully!";
                ShowSuccessMessage(message);
            }
            else
            {
                ShowErrorMessage(result.Error.Description);
            }
        }

        private async Task ConfirmDeleteOperation(Guid operationId, string operationName)
        {
            var parameters = new DialogParameters
        {
            { "ContentText", $"Are you sure you want to delete the operation '{operationName}'? This action cannot be undone." },
            { "ButtonText", "Delete" },
            { "Color", Color.Error }
        };

            var dialog = await DialogService.ShowAsync<ConfirmDialogComponent>("Confirm Delete", parameters);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                await DeleteOperation(operationId);
            }
        }

        private async Task DeleteOperation(Guid operationId)
        {
            var command = new DeleteOperationCommand { Id = operationId };
            var result = await Mediator.Send(command);

            if (result.IsSuccess)
            {
                await LoadOperations();
                ShowSuccessMessage("Operation deleted successfully!");
            }
            else
            {
                ShowErrorMessage(result.Error.Description);
            }
        }

        private void ShowSuccessMessage(string message)
        {
            Snackbar.Clear();
            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
            Snackbar.Add(message, Severity.Success);
        }

        private void ShowErrorMessage(string message)
        {
            Snackbar.Clear();
            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
            Snackbar.Add(message, Severity.Error);
        }

        private void Cancel() => MudDialog?.Cancel();



    }

}
