# Guia Técnico - Implementação Step-by-Step

## Overview
Este documento detalha os passos específicos para implementar a feature de Operations Management em 3ASystem.

---

## PART 1: DOMAIN LAYER

### Step 1.1: Criar OperationErrors.cs

**Caminho**: `src\3ASystem.Domain\Entities\Operations\OperationErrors.cs`

```csharp
namespace _3ASystem.Domain.Entities.Operations;

public static class OperationErrors
{
    public static Error NotFound(OperationId id) =>
        Error.NotFound("Operation.NotFound", $"The operation with ID '{id}' was not found.");

    public static Error CodeNotUnique =>
        Error.Conflict("Operation.CodeNotUnique", "An operation with the same code already exists.");

    public static Error FriendlyIdNotUnique =>
        Error.Conflict("Operation.FriendlyIdNotUnique", "An operation with the same friendly ID already exists.");

    public static Error InvalidFunctionalityId =>
        Error.Validation("Operation.InvalidFunctionalityId", "The specified functionality does not exist.");

    public static Error CannotDeleteOperation =>
        Error.Conflict("Operation.CannotDelete", "This operation cannot be deleted as it has dependencies.");
}
```

**Padrão**: Seguir `ModuleErrors`, `FunctionalityErrors`, `ApplicationErrors`

### Step 1.2: Validar Entidade Operation.cs

Verificar se `Operation.cs` já contém:
- ✓ Métodos privados: Constructor, Create
- ✓ Propriedades: Id, FunctionalityId, Name, Code, IsActive, FriendlyId
- ✓ Métodos de domínio: Update, Enable, Disable
- ✓ Relacionamento: Functionality

**Arquivo**: `src\3ASystem.Domain\Entities\Operations\Operation.cs`

Se faltar `Update`, adicionar:
```csharp
public void Update(string name, string code, string friendlyId)
{
    Name = name;
    Code = code;
    FriendlyId = friendlyId;
    LastUpdatedAt = DateTime.Now;
}
```

---

## PART 2: APPLICATION LAYER

### Step 2.1: Create Operation Command

**Caminho**: `src\3ASystem.Application\UseCases\Operations\Commands\CreateOperation\`

#### CreateOperationCommand.cs
```csharp
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Operations.Responses;

namespace _3ASystem.Application.UseCases.Operations.Commands.CreateOperation;

public sealed class CreateOperationCommand : ICommand<OperationResponse>
{
    public Guid FunctionalityId { get; set; }
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string FriendlyId { get; set; } = default!;
}
```

#### CreateOperationCommandValidator.cs
```csharp
using FluentValidation;

namespace _3ASystem.Application.UseCases.Operations.Commands.CreateOperation;

public sealed class CreateOperationCommandValidator : AbstractValidator<CreateOperationCommand>
{
    public CreateOperationCommandValidator()
    {
        RuleFor(x => x.FunctionalityId)
            .NotEmpty()
            .WithMessage("Functionality ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Operation name is required.")
            .MaximumLength(100)
            .WithMessage("Operation name cannot exceed 100 characters.");

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Operation code is required.")
            .MaximumLength(50)
            .WithMessage("Operation code cannot exceed 50 characters.");

        RuleFor(x => x.FriendlyId)
            .NotEmpty()
            .WithMessage("Friendly ID is required.")
            .MaximumLength(50)
            .WithMessage("Friendly ID cannot exceed 50 characters.");
    }
}
```

#### CreateOperationCommandHandler.cs
```csharp
using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Operations.Responses;
using _3ASystem.Domain.Entities.Functionalities;
using _3ASystem.Domain.Entities.Operations;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Operations.Commands.CreateOperation;

public sealed class CreateOperationCommandHandler : ICommandHandler<CreateOperationCommand, OperationResponse>
{
    private readonly IOperationRepository _operationRepository;
    private readonly IFunctionalityRepository _functionalityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOperationCommandHandler(
        IOperationRepository operationRepository,
        IFunctionalityRepository functionalityRepository,
        IUnitOfWork unitOfWork)
    {
        _operationRepository = operationRepository;
        _functionalityRepository = functionalityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<OperationResponse>> Handle(
        CreateOperationCommand request,
        CancellationToken cancellationToken)
    {
        // Validate functionality exists
        var functionalityId = new FunctionalityId(request.FunctionalityId);
        var functionality = await _functionalityRepository.GetByIdAsync(functionalityId);
        if (functionality is null)
            return Result.Failure<OperationResponse>(OperationErrors.InvalidFunctionalityId);

        // Check code uniqueness
        var existingByCode = await _operationRepository.GetByCodeAsync(request.Code);
        if (existingByCode is not null)
            return Result.Failure<OperationResponse>(OperationErrors.CodeNotUnique);

        // Check friendly ID uniqueness
        var existingByFriendlyId = await _operationRepository.GetByFriendlyIdAsync(request.FriendlyId);
        if (existingByFriendlyId is not null)
            return Result.Failure<OperationResponse>(OperationErrors.FriendlyIdNotUnique);

        // Create operation
        var operation = Operation.Create(
            functionalityId,
            request.Name,
            request.Code,
            request.FriendlyId);

        _operationRepository.Create(operation);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return operation.ToOperationResponse();
    }
}
```

**Padrão**: Copiar estrutura de `CreateFunctionalityCommandHandler`

### Step 2.2: Update Operation Command

**Caminho**: `src\3ASystem.Application\UseCases\Operations\Commands\UpdateOperation\`

#### UpdateOperationCommand.cs
```csharp
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Operations.Responses;

namespace _3ASystem.Application.UseCases.Operations.Commands.UpdateOperation;

public sealed class UpdateOperationCommand : ICommand<OperationResponse>
{
    public Guid OperationId { get; set; }
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string FriendlyId { get; set; } = default!;
}
```

#### UpdateOperationCommandValidator.cs
```csharp
using FluentValidation;

namespace _3ASystem.Application.UseCases.Operations.Commands.UpdateOperation;

public sealed class UpdateOperationCommandValidator : AbstractValidator<UpdateOperationCommand>
{
    public UpdateOperationCommandValidator()
    {
        RuleFor(x => x.OperationId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Code).NotEmpty().MaximumLength(50);
        RuleFor(x => x.FriendlyId).NotEmpty().MaximumLength(50);
    }
}
```

#### UpdateOperationCommandHandler.cs
```csharp
using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Operations.Responses;
using _3ASystem.Domain.Entities.Operations;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Operations.Commands.UpdateOperation;

public sealed class UpdateOperationCommandHandler : ICommandHandler<UpdateOperationCommand, OperationResponse>
{
    private readonly IOperationRepository _operationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateOperationCommandHandler(
        IOperationRepository operationRepository,
        IUnitOfWork unitOfWork)
    {
        _operationRepository = operationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<OperationResponse>> Handle(
        UpdateOperationCommand request,
        CancellationToken cancellationToken)
    {
        var operationId = new OperationId(request.OperationId);
        var operation = await _operationRepository.GetByIdAsync(operationId);

        if (operation is null)
            return Result.Failure<OperationResponse>(OperationErrors.NotFound(operationId));

        // Check code uniqueness (excluding current record)
        var existingByCode = await _operationRepository.GetByCodeAsync(request.Code);
        if (existingByCode is not null && !existingByCode.Id.Equals(operationId))
            return Result.Failure<OperationResponse>(OperationErrors.CodeNotUnique);

        // Check friendly ID uniqueness (excluding current record)
        var existingByFriendlyId = await _operationRepository.GetByFriendlyIdAsync(request.FriendlyId);
        if (existingByFriendlyId is not null && !existingByFriendlyId.Id.Equals(operationId))
            return Result.Failure<OperationResponse>(OperationErrors.FriendlyIdNotUnique);

        operation.Update(request.Name, request.Code, request.FriendlyId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return operation.ToOperationResponse();
    }
}
```

### Step 2.3: Delete Operation Command

**Caminho**: `src\3ASystem.Application\UseCases\Operations\Commands\DeleteOperation\`

#### DeleteOperationCommand.cs
```csharp
using _3ASystem.Application.Abstractions.Messaging;

namespace _3ASystem.Application.UseCases.Operations.Commands.DeleteOperation;

public sealed class DeleteOperationCommand : ICommand
{
    public Guid OperationId { get; set; }
}
```

#### DeleteOperationCommandValidator.cs
```csharp
using FluentValidation;

namespace _3ASystem.Application.UseCases.Operations.Commands.DeleteOperation;

public sealed class DeleteOperationCommandValidator : AbstractValidator<DeleteOperationCommand>
{
    public DeleteOperationCommandValidator()
    {
        RuleFor(x => x.OperationId).NotEmpty();
    }
}
```

#### DeleteOperationCommandHandler.cs
```csharp
using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Domain.Entities.Operations;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Operations.Commands.DeleteOperation;

public sealed class DeleteOperationCommandHandler : ICommandHandler<DeleteOperationCommand>
{
    private readonly IOperationRepository _operationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteOperationCommandHandler(
        IOperationRepository operationRepository,
        IUnitOfWork unitOfWork)
    {
        _operationRepository = operationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        DeleteOperationCommand request,
        CancellationToken cancellationToken)
    {
        var operationId = new OperationId(request.OperationId);
        var operation = await _operationRepository.GetByIdAsync(operationId);

        if (operation is null)
            return Result.Failure(OperationErrors.NotFound(operationId));

        // TODO: Check if operation has dependencies (RoleOperations, etc.)
        // if (hasDependencies)
        //     return Result.Failure(OperationErrors.CannotDeleteOperation);

        _operationRepository.Delete(operation);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
```

### Step 2.4: Enable/Disable Operation Command

**Caminho**: `src\3ASystem.Application\UseCases\Operations\Commands\EnableDisableOperation\`

#### EnableDisableOperationCommand.cs
```csharp
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Operations.Responses;

namespace _3ASystem.Application.UseCases.Operations.Commands.EnableDisableOperation;

public sealed class EnableDisableOperationCommand : ICommand<OperationResponse>
{
    public Guid OperationId { get; set; }
    public bool IsActive { get; set; }
}
```

#### EnableDisableOperationCommandHandler.cs
```csharp
using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Operations.Responses;
using _3ASystem.Domain.Entities.Operations;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Operations.Commands.EnableDisableOperation;

public sealed class EnableDisableOperationCommandHandler : 
    ICommandHandler<EnableDisableOperationCommand, OperationResponse>
{
    private readonly IOperationRepository _operationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EnableDisableOperationCommandHandler(
        IOperationRepository operationRepository,
        IUnitOfWork unitOfWork)
    {
        _operationRepository = operationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<OperationResponse>> Handle(
        EnableDisableOperationCommand request,
        CancellationToken cancellationToken)
    {
        var operationId = new OperationId(request.OperationId);
        var operation = await _operationRepository.GetByIdAsync(operationId);

        if (operation is null)
            return Result.Failure<OperationResponse>(OperationErrors.NotFound(operationId));

        if (request.IsActive)
            operation.Enable();
        else
            operation.Disable();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return operation.ToOperationResponse();
    }
}
```

### Step 2.5: Get Operations Queries

**Caminho**: `src\3ASystem.Application\UseCases\Operations\Queries\`

#### GetOperationsByFunctionalityQuery.cs
```csharp
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Operations.Responses;

namespace _3ASystem.Application.UseCases.Operations.Queries.GetOperationsByFunctionality;

public sealed class GetOperationsByFunctionalityQuery : IQuery<List<OperationResponse>>
{
    public Guid FunctionalityId { get; set; }

    public GetOperationsByFunctionalityQuery(Guid functionalityId)
    {
        FunctionalityId = functionalityId;
    }
}
```

#### GetOperationsByFunctionalityHandler.cs
```csharp
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Operations.Responses;
using _3ASystem.Domain.Entities.Functionalities;
using _3ASystem.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace _3ASystem.Application.UseCases.Operations.Queries.GetOperationsByFunctionality;

public sealed class GetOperationsByFunctionalityHandler : 
    IQueryHandler<GetOperationsByFunctionalityQuery, List<OperationResponse>>
{
    private readonly IOperationRepository _operationRepository;

    public GetOperationsByFunctionalityHandler(IOperationRepository operationRepository)
    {
        _operationRepository = operationRepository;
    }

    public async Task<Result<List<OperationResponse>>> Handle(
        GetOperationsByFunctionalityQuery request,
        CancellationToken cancellationToken)
    {
        var functionalityId = new FunctionalityId(request.FunctionalityId);

        // Assuming OperationRepository has a method to get by FunctionalityId
        var operations = await _operationRepository
            .Entity
            .AsNoTracking()
            .Where(o => o.FunctionalityId == functionalityId)
            .OrderBy(o => o.CreatedAt)
            .ToListAsync(cancellationToken);

        var responses = operations.Select(o => o.ToOperationResponse()).ToList();
        return responses;
    }
}
```

### Step 2.6: Operation Response DTOs

**Caminho**: `src\3ASystem.Application\UseCases\Operations\Responses\`

#### OperationResponse.cs
```csharp
namespace _3ASystem.Application.UseCases.Operations.Responses;

public sealed class OperationResponse
{
    public Guid Id { get; init; }
    public Guid FunctionalityId { get; init; }
    public string Name { get; init; } = default!;
    public string Code { get; init; } = default!;
    public bool IsActive { get; init; }
    public string FriendlyId { get; init; } = default!;
    public DateTime CreatedAt { get; init; }
    public DateTime? LastUpdatedAt { get; init; }
}
```

#### OperationResponseExtensions.cs
```csharp
using _3ASystem.Domain.Entities.Operations;

namespace _3ASystem.Application.UseCases.Operations.Responses;

public static class OperationResponseExtensions
{
    public static OperationResponse ToOperationResponse(this Operation operation)
    {
        return new OperationResponse
        {
            Id = operation.Id.Value,
            FunctionalityId = operation.FunctionalityId.Value,
            Name = operation.Name,
            Code = operation.Code,
            IsActive = operation.IsActive,
            FriendlyId = operation.FriendlyId,
            CreatedAt = operation.CreatedAt,
            LastUpdatedAt = operation.LastUpdatedAt
        };
    }
}
```

### Step 2.7: Expandir OperationRepository

**Caminho**: `src\3ASystem.Infrastructure\Data\Repositories\OperationRepository.cs`

Adicionar métodos auxiliares:

```csharp
using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Domain.Entities.Operations;
using Microsoft.EntityFrameworkCore;

namespace _3ASystem.Infrastructure.Data.Repositories;

public sealed class OperationRepository : _Repository<Operation, OperationId>, IOperationRepository
{
    public OperationRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<Operation?> GetByCodeAsync(string code)
    {
        var operation = await Entity.AsNoTracking()
            .FirstOrDefaultAsync(o => o.Code == code);
        return operation;
    }

    public async Task<Operation?> GetByFriendlyIdAsync(string friendlyId)
    {
        var operation = await Entity.AsNoTracking()
            .FirstOrDefaultAsync(o => o.FriendlyId == friendlyId);
        return operation;
    }
}
```

### Step 2.8: Atualizar IOperationRepository

**Caminho**: `src\3ASystem.Application\Abstractions\Data\Repositories\IOperationRepository.cs`

```csharp
using _3ASystem.Domain.Entities.Operations;

namespace _3ASystem.Application.Abstractions.Data.Repositories;

public interface IOperationRepository : IRepository<Operation, OperationId>
{
    Task<Operation?> GetByCodeAsync(string code);
    Task<Operation?> GetByFriendlyIdAsync(string friendlyId);
}
```

---

## PART 3: INFRASTRUCTURE - MediatR REGISTRATION

Verificar se em `Program.cs` já existe registro automático de handlers via reflection scanning.

Típicamente:
```csharp
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssemblyContaining<CreateApplicationCommandHandler>());
```

Se não existir, adicionar:
```csharp
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssemblies(
        typeof(CreateOperationCommandHandler).Assembly));
```

---

## PART 4: WEB UI COMPONENTS

### Step 4.1: Criar OperationsModal.razor

**Caminho**: `src\3ASystem.WebUI.Server\Components\Pages\Functionalities\OperationsModal.razor`

```razor
@namespace _3ASystem.WebUI.Server.Components.Pages.Functionalities
@rendermode @(new InteractiveServerRenderMode(prerender: false))

<div class="operations-modal-container">
    <!-- Title Section -->
    <MudText Typo="Typo.h6" GutterBottom="true">
        <MudIcon Icon="@Icons.Material.Filled.Assignment" Class="mr-2" />
        Gerenciar Operações: @FunctionalityName
    </MudText>

    <MudDivider Class="my-4" />

    <!-- Standard Operations Section -->
    <div class="operations-section">
        <MudText Typo="Typo.subtitle2" Class="section-title">
            Operações Padrão
        </MudText>
        <MudButton 
            Variant="Variant.Filled" 
            Color="Color.Success"
            Class="standard-operations-button"
            OnClick="AddStandardOperationsAsync"
            Disabled="_addStandardButtonDisabled || _isProcessing">
            <MudIcon Icon="@Icons.Material.Filled.CheckCircle" Class="mr-2" />
            Adicionar Operações Padrão
        </MudButton>
    </div>

    <!-- Operations Table Section -->
    <div class="operations-section">
        <MudText Typo="Typo.subtitle2" Class="section-title">
            Operações Cadastradas
        </MudText>

        @if (_isLoading)
        {
            <MudProgressLinear Color="Color.Info" Indeterminate="true" />
        }
        else if (_operations != null && _operations.Any())
        {
            <MudTable Items="@_operations" FixedHeader="true" Dense="true" Hover="true" Bordered="false" Striped="true" Class="operations-table">
                <HeaderContent>
                    <MudTh Style="width: 25%; font-weight:bold;">Name</MudTh>
                    <MudTh Style="width: 15%; font-weight:bold;">Code</MudTh>
                    <MudTh Style="width: 20%; font-weight:bold;">Friendly ID</MudTh>
                    <MudTh Style="width: 15%; font-weight:bold;text-align:center;">Active</MudTh>
                    <MudTh Style="width: 25%; font-weight:bold;text-align:center;">Actions</MudTh>
                </HeaderContent>
                <RowTemplate>
                    <MudTd DataLabel="Name">@context.Name</MudTd>
                    <MudTd DataLabel="Code">@context.Code</MudTd>
                    <MudTd DataLabel="Friendly ID">@context.FriendlyId</MudTd>
                    <MudTd DataLabel="Active" Style="text-align:center;">
                        @if (context.IsActive)
                        {
                            <MudButton Size="Size.Small" Variant="Variant.Outlined" Color="Color.Primary" 
                                OnClick="() => ToggleOperationAsync(context.Id)" Class="mr-1">
                                <MudIcon Icon="@Icons.Material.Filled.CheckCircle" Color="Color.Warning" Title="Disable"></MudIcon>
                            </MudButton>
                        }
                        else
                        {
                            <MudButton Size="Size.Small" Variant="Variant.Outlined" Color="Color.Primary" 
                                OnClick="() => ToggleOperationAsync(context.Id)" Class="mr-1">
                                <MudIcon Icon="@Icons.Material.Filled.CheckCircle" Color="Color.Tertiary" Title="Enable"></MudIcon>
                            </MudButton>
                        }
                    </MudTd>
                    <MudTd DataLabel="Actions" Style="text-align:center;">
                        <MudButton Size="Size.Small" Variant="Variant.Outlined" Color="Color.Primary" 
                            OnClick="() => EditOperationAsync(context)" Class="mr-1">
                            <MudIcon Icon="@Icons.Material.Filled.Edit" Color="Color.Info" Title="Edit"></MudIcon>
                        </MudButton>
                        <MudButton Size="Size.Small" Variant="Variant.Outlined" Color="Color.Primary" 
                            OnClick="() => AskConfirmDeleteAsync(context.Id, context.Name)">
                            <MudIcon Icon="@Icons.Material.Filled.Delete" Color="Color.Error" Title="Delete"></MudIcon>
                        </MudButton>
                    </MudTd>
                </RowTemplate>
            </MudTable>
        }
        else
        {
            <MudAlert Severity="Severity.Info">No operations found for this functionality.</MudAlert>
        }
    </div>

    <!-- Form Section (Show/Hide) -->
    @if (_showForm)
    {
        <div class="operation-form-container">
            <OperationForm 
                @ref="_operationForm"
                Mode="_formMode"
                Operation="_editingOperation"
                FunctionalityId="@FunctionalityId"
                OnSave="HandleFormSaveAsync"
                OnCancel="HandleFormCancelAsync" />
        </div>
    }

    <!-- Error Display -->
    @if (!string.IsNullOrEmpty(_error))
    {
        <MudAlert Severity="Severity.Error" Class="mt-3">
            @_error
        </MudAlert>
    }
</div>

@code {
    [Parameter]
    public Guid FunctionalityId { get; set; }

    [Parameter]
    public string FunctionalityName { get; set; } = default!;

    [Inject]
    public IMediator Mediator { get; set; } = default!;

    [Inject]
    public ISnackbar Snackbar { get; set; } = default!;

    [Inject]
    public IDialogService DialogService { get; set; } = default!;

    private List<OperationResponse>? _operations = new();
    private bool _isLoading = false;
    private bool _isProcessing = false;
    private bool _showForm = false;
    private string _error = string.Empty;
    private OperationResponse? _editingOperation;
    private OperationForm? _operationForm;
    private string _formMode = "Create";
    private bool _addStandardButtonDisabled = false;

    private const string VIEW_CODE = "VIEW";
    private const string CREATE_CODE = "CREATE";
    private const string UPDATE_CODE = "UPDATE";
    private const string DELETE_CODE = "DELETE";

    protected override async Task OnInitializedAsync()
    {
        await LoadOperationsAsync();
    }

    private async Task LoadOperationsAsync()
    {
        try
        {
            _isLoading = true;
            _error = string.Empty;

            var query = new GetOperationsByFunctionalityQuery(FunctionalityId);
            var result = await Mediator.Send(query);

            if (result.IsSuccess)
            {
                _operations = result.Value ?? new();
                CheckStandardOperationsExist();
            }
            else
            {
                _error = result.Error?.Description ?? "Error loading operations.";
            }
        }
        catch (Exception ex)
        {
            _error = $"An error occurred: {ex.Message}";
        }
        finally
        {
            _isLoading = false;
        }
    }

    private void CheckStandardOperationsExist()
    {
        var standardCodes = new[] { VIEW_CODE, CREATE_CODE, UPDATE_CODE, DELETE_CODE };
        var existingCodes = _operations?.Select(o => o.Code.ToUpper()).ToList() ?? new();

        _addStandardButtonDisabled = standardCodes.All(code => existingCodes.Contains(code));
    }

    private async Task AddStandardOperationsAsync()
    {
        try
        {
            _isProcessing = true;
            _error = string.Empty;

            var standardOps = new[]
            {
                ("View", VIEW_CODE, "view"),
                ("Create", CREATE_CODE, "create"),
                ("Update", UPDATE_CODE, "update"),
                ("Delete", DELETE_CODE, "delete")
            };

            // Check which ones already exist
            var existingCodes = _operations?.Select(o => o.Code.ToUpper()).ToHashSet() ?? new();
            var toAdd = standardOps.Where(op => !existingCodes.Contains(op.Item2)).ToList();

            if (toAdd.Count == 0)
            {
                Snackbar.Add("All standard operations already exist.", Severity.Warning);
                return;
            }

            int successCount = 0;
            foreach (var (name, code, friendlyId) in toAdd)
            {
                var command = new CreateOperationCommand
                {
                    FunctionalityId = FunctionalityId,
                    Name = name,
                    Code = code,
                    FriendlyId = friendlyId
                };

                var result = await Mediator.Send(command);
                if (result.IsSuccess)
                    successCount++;
                else
                    _error = result.Error?.Description ?? "Error adding standard operations.";
            }

            if (successCount > 0)
            {
                await LoadOperationsAsync();
                Snackbar.Add($"{successCount} standard operation(s) added successfully!", Severity.Success);
            }
        }
        catch (Exception ex)
        {
            _error = $"An error occurred: {ex.Message}";
        }
        finally
        {
            _isProcessing = false;
        }
    }

    private async Task ToggleOperationAsync(Guid operationId)
    {
        try
        {
            _isProcessing = true;
            _error = string.Empty;

            var operation = _operations?.FirstOrDefault(o => o.Id == operationId);
            if (operation == null)
                return;

            var command = new EnableDisableOperationCommand
            {
                OperationId = operationId,
                IsActive = !operation.IsActive
            };

            var result = await Mediator.Send(command);

            if (result.IsSuccess)
            {
                await LoadOperationsAsync();
                var action = result.Value!.IsActive ? "activated" : "deactivated";
                Snackbar.Add($"Operation {action} successfully!", Severity.Success);
            }
            else
            {
                _error = result.Error?.Description ?? "Error toggling operation.";
            }
        }
        catch (Exception ex)
        {
            _error = $"An error occurred: {ex.Message}";
        }
        finally
        {
            _isProcessing = false;
        }
    }

    private void EditOperationAsync(OperationResponse operation)
    {
        _editingOperation = operation;
        _formMode = "Edit";
        _showForm = true;
    }

    private async Task AskConfirmDeleteAsync(Guid operationId, string operationName)
    {
        var parameters = new DialogParameters<ConfirmDialog>();
        parameters.Add(x => x.ContentText, $"Are you sure you want to delete the operation '{operationName}'?");
        parameters.Add(x => x.ButtonText, "Delete");
        parameters.Add(x => x.Color, Color.Error);

        var dialog = await DialogService.ShowAsync<ConfirmDialog>("Confirm Delete", parameters);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await DeleteOperationAsync(operationId);
        }
    }

    private async Task DeleteOperationAsync(Guid operationId)
    {
        try
        {
            _isProcessing = true;
            _error = string.Empty;

            var command = new DeleteOperationCommand { OperationId = operationId };
            var result = await Mediator.Send(command);

            if (result.IsSuccess)
            {
                await LoadOperationsAsync();
                Snackbar.Add("Operation deleted successfully!", Severity.Success);
            }
            else
            {
                _error = result.Error?.Description ?? "Error deleting operation.";
            }
        }
        catch (Exception ex)
        {
            _error = $"An error occurred: {ex.Message}";
        }
        finally
        {
            _isProcessing = false;
        }
    }

    private async Task HandleFormSaveAsync()
    {
        await LoadOperationsAsync();
        _showForm = false;
    }

    private void HandleFormCancelAsync()
    {
        _showForm = false;
        _editingOperation = null;
        _error = string.Empty;
    }
}
```

### Step 4.2: Criar OperationForm.razor

**Caminho**: `src\3ASystem.WebUI.Server\Components\Pages\Functionalities\OperationForm.razor`

```razor
@namespace _3ASystem.WebUI.Server.Components.Pages.Functionalities
@rendermode @(new InteractiveServerRenderMode(prerender: false))

<EditForm Model="@_model">
    <DataAnnotationsValidator />

    <MudGrid>
        <MudItem md="12">
            <MudTextField 
                Label="Operation Name"
                @bind-Value="_model.Name"
                For="@(() => _model.Name)"
                Variant="Variant.Outlined"
                FullWidth="true"
                TextChanged="() => ClearError()"
                Class="mb-3" />
        </MudItem>

        <MudItem md="6">
            <MudTextField 
                Label="Code"
                @bind-Value="_model.Code"
                For="@(() => _model.Code)"
                Variant="Variant.Outlined"
                FullWidth="true"
                TextChanged="() => ClearError()"
                Class="mb-3" />
        </MudItem>

        <MudItem md="6">
            <MudTextField 
                Label="Friendly ID"
                @bind-Value="_model.FriendlyId"
                For="@(() => _model.FriendlyId)"
                Variant="Variant.Outlined"
                FullWidth="true"
                TextChanged="() => ClearError()"
                Class="mb-3" />
        </MudItem>

        @if (!string.IsNullOrEmpty(_error))
        {
            <MudItem md="12">
                <MudAlert Severity="Severity.Error">@_error</MudAlert>
            </MudItem>
        }

        <MudItem md="12" Class="d-flex justify-end gap-3 mt-4">
            <MudButton 
                Variant="Variant.Outlined" 
                Color="Color.Secondary"
                OnClick="Cancel"
                Disabled="_isProcessing">
                Cancel
            </MudButton>
            <MudButton 
                Variant="Variant.Filled" 
                Color="Color.Primary"
                ButtonType="ButtonType.Submit"
                OnClick="SaveAsync"
                Disabled="_isProcessing">
                @if (_isProcessing)
                {
                    <MudProgressCircular Size="Size.Small" Indeterminate="true" Class="mr-2" />
                }
                Save
            </MudButton>
        </MudItem>
    </MudGrid>
</EditForm>

@code {
    [Parameter]
    public string Mode { get; set; } = "Create";

    [Parameter]
    public OperationResponse? Operation { get; set; }

    [Parameter]
    public Guid FunctionalityId { get; set; }

    [Parameter]
    public EventCallback OnSave { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    [Inject]
    public IMediator Mediator { get; set; } = default!;

    [Inject]
    public ISnackbar Snackbar { get; set; } = default!;

    private OperationFormModel _model = new();
    private bool _isProcessing = false;
    private string _error = string.Empty;

    protected override void OnInitialized()
    {
        if (Mode == "Edit" && Operation != null)
        {
            _model = new OperationFormModel
            {
                Name = Operation.Name,
                Code = Operation.Code,
                FriendlyId = Operation.FriendlyId
            };
        }
    }

    private async Task SaveAsync()
    {
        try
        {
            _isProcessing = true;
            _error = string.Empty;

            if (Mode == "Create")
            {
                var command = new CreateOperationCommand
                {
                    FunctionalityId = FunctionalityId,
                    Name = _model.Name!,
                    Code = _model.Code!,
                    FriendlyId = _model.FriendlyId!
                };

                var result = await Mediator.Send(command);

                if (result.IsSuccess)
                {
                    Snackbar.Add("Operation created successfully!", Severity.Success);
                    await OnSave.InvokeAsync();
                }
                else
                {
                    _error = result.Error?.Description ?? "Error creating operation.";
                }
            }
            else if (Mode == "Edit" && Operation != null)
            {
                var command = new UpdateOperationCommand
                {
                    OperationId = Operation.Id,
                    Name = _model.Name!,
                    Code = _model.Code!,
                    FriendlyId = _model.FriendlyId!
                };

                var result = await Mediator.Send(command);

                if (result.IsSuccess)
                {
                    Snackbar.Add("Operation updated successfully!", Severity.Success);
                    await OnSave.InvokeAsync();
                }
                else
                {
                    _error = result.Error?.Description ?? "Error updating operation.";
                }
            }
        }
        catch (Exception ex)
        {
            _error = $"An error occurred: {ex.Message}";
        }
        finally
        {
            _isProcessing = false;
        }
    }

    private async Task Cancel()
    {
        await OnCancel.InvokeAsync();
    }

    private void ClearError()
    {
        _error = string.Empty;
    }

    private class OperationFormModel
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? FriendlyId { get; set; }
    }
}
```

### Step 4.3: Criar OperationsModal.razor.css

**Caminho**: `src\3ASystem.WebUI.Server\Components\Pages\Functionalities\OperationsModal.razor.css`

```css
.operations-modal-container {
    min-width: 600px;
    max-height: 80vh;
    overflow-y: auto;
    padding: 8px;
}

.operations-table {
    margin-top: 16px;
    margin-bottom: 16px;
}

.operations-section {
    margin-bottom: 24px;
}

.section-title {
    font-weight: 600;
    font-size: 1rem;
    margin-bottom: 12px;
    color: rgba(0, 0, 0, 0.87);
}

.standard-operations-button {
    margin-bottom: 16px;
}

.operation-form-container {
    background-color: #fafafa;
    padding: 16px;
    border-radius: 4px;
    border: 1px solid #e0e0e0;
    margin-top: 16px;
}

.gap-3 {
    gap: 12px;
}
```

### Step 4.4: Criar OperationForm.razor.css

**Caminho**: `src\3ASystem.WebUI.Server\Components\Pages\Functionalities\OperationForm.razor.css`

```css
.mb-3 {
    margin-bottom: 16px;
}

.mt-4 {
    margin-top: 24px;
}

.d-flex {
    display: flex;
}

.justify-end {
    justify-content: flex-end;
}

.gap-3 {
    gap: 12px;
}

.mr-2 {
    margin-right: 8px;
}
```

### Step 4.5: Modificar FunctionalitiesList.razor

No arquivo `src\3ASystem.WebUI.Server\Components\Pages\Functionalities\FunctionalitiesList.razor`, adicionar botão "Gerenciar Operações" na seção de ações:

```razor
<!-- Em RowTemplate, dentro da seção MudTd de Actions, adicionar: -->
<MudButton Size="Size.Small" Variant="Variant.Outlined" Color="Color.Primary" 
    OnClick="() => OpenOperationsModal(context.Id, context.Name)" Class="mr-1">
    <MudIcon Icon="@Icons.Material.Filled.Tune" Color="Color.Primary" Title="Manage Operations"></MudIcon>
</MudButton>
```

### Step 4.6: Modificar FunctionalitiesList.razor.cs

Adicionar método para abrir modal:

```csharp
private IDialogReference dlgOperations = default!;

private async Task OpenOperationsModal(Guid functionalityId, string functionalityName)
{
    var parameters = new DialogParameters();
    parameters.Add("FunctionalityId", functionalityId);
    parameters.Add("FunctionalityName", functionalityName);

    dlgOperations = await DialogService.ShowAsync<OperationsModal>(
        "Manage Operations",
        parameters,
        new DialogOptions { MaxWidth = MaxWidth.Large, FullWidth = true });

    var result = await dlgOperations.Result;
    if (!result.Canceled)
    {
        // Reload if needed
        await LoadFunctionalitiesAsync();
    }
}
```

---

## PART 5: TESTING

### Unit Tests (Domain Layer)

**Arquivo**: `tests\3ASystem.Tests.Domain\Entities\Operations\OperationTests.cs`

```csharp
namespace _3ASystem.Tests.Domain.Entities.Operations;

public class OperationTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateOperation()
    {
        // Arrange
        var functionalityId = new FunctionalityId(Guid.NewGuid());
        var name = "View";
        var code = "VIEW";
        var friendlyId = "view";

        // Act
        var operation = Operation.Create(functionalityId, name, code, friendlyId);

        // Assert
        Assert.NotNull(operation);
        Assert.Equal(name, operation.Name);
        Assert.Equal(code, operation.Code);
        Assert.Equal(friendlyId, operation.FriendlyId);
        Assert.True(operation.IsActive);
    }

    [Fact]
    public void Update_WithValidData_ShouldUpdateOperation()
    {
        // Arrange
        var operation = Operation.Create(
            new FunctionalityId(Guid.NewGuid()),
            "View",
            "VIEW",
            "view");

        // Act
        operation.Update("View All", "VIEWALL", "viewall");

        // Assert
        Assert.Equal("View All", operation.Name);
        Assert.Equal("VIEWALL", operation.Code);
        Assert.Equal("viewall", operation.FriendlyId);
    }

    [Fact]
    public void Enable_ShouldSetIsActiveToTrue()
    {
        // Arrange
        var operation = Operation.Create(
            new FunctionalityId(Guid.NewGuid()),
            "View",
            "VIEW",
            "view");
        operation.Disable();

        // Act
        operation.Enable();

        // Assert
        Assert.True(operation.IsActive);
    }

    [Fact]
    public void Disable_ShouldSetIsActiveToFalse()
    {
        // Arrange
        var operation = Operation.Create(
            new FunctionalityId(Guid.NewGuid()),
            "View",
            "VIEW",
            "view");

        // Act
        operation.Disable();

        // Assert
        Assert.False(operation.IsActive);
    }
}
```

### Integration Tests (Application Layer)

**Arquivo**: `tests\3ASystem.Tests.Application\Application\Commands\CreateOperationCommandHandlerTests.cs`

```csharp
namespace _3ASystem.Tests.Application.Application.Commands;

public class CreateOperationCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithValidData_ShouldCreateOperation()
    {
        // Arrange
        var handler = new CreateOperationCommandHandler(
            operationRepository,
            functionalityRepository,
            unitOfWork);

        var command = new CreateOperationCommand
        {
            FunctionalityId = Guid.NewGuid(),
            Name = "View",
            Code = "VIEW",
            FriendlyId = "view"
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task Handle_WithDuplicateCode_ShouldReturnFailure()
    {
        // Arrange
        // Setup existing operation with code "VIEW"
        // ...

        var command = new CreateOperationCommand
        {
            FunctionalityId = Guid.NewGuid(),
            Name = "View All",
            Code = "VIEW", // Duplicate
            FriendlyId = "viewall"
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Operation.CodeNotUnique", result.Error?.Code);
    }
}
```

---

## PART 6: VERIFICAÇÃO FINAL

### Checklist de Validação

- [ ] Todos os arquivos criados em suas respectivas pastas
- [ ] Imports corretos em todos os arquivos
- [ ] IOperationRepository contém novos métodos
- [ ] GetOperationsByFunctionalityHandler acessa corretamente
- [ ] MediatR handlers registrados automaticamente
- [ ] OperationsModal abre via botão em FunctionalitiesList
- [ ] CRUD completo funciona
- [ ] Validações de negócio funcionam
- [ ] Mensagens de feedback exibem corretamente
- [ ] Operações padrão adicionadas sem duplicatas
- [ ] Toggle ativar/desativar funciona
- [ ] Confirmação de deleção funciona
- [ ] Responsividade mobile/tablet
- [ ] Testes unitários passam
- [ ] Testes integração passam

---

**FIM DO GUIA TÉCNICO**

Este guia registra a abordagem técnica da feature. O código já está implementado; use `IMPLEMENTATION_SUMMARY.md` para acompanhar a migração pendente e a validação manual.
