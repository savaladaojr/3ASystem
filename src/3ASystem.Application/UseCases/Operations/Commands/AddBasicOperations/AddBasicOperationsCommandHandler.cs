using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Operations.Responses;
using _3ASystem.Domain.Entities.Functionalities;
using _3ASystem.Domain.Entities.Operations;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Operations.Commands.AddBasicOperations;

public class AddBasicOperationsCommandHandler : ICommandHandler<AddBasicOperationsCommand, List<OperationDetailedResponse>>
{
    private readonly IOperationRepository _operationRepository;
    private readonly IFunctionalityRepository _functionalityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddBasicOperationsCommandHandler(
        IOperationRepository operationRepository,
        IFunctionalityRepository functionalityRepository,
        IUnitOfWork unitOfWork)
    {
        _operationRepository = operationRepository;
        _functionalityRepository = functionalityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<OperationDetailedResponse>>> Handle(AddBasicOperationsCommand request, CancellationToken cancellationToken)
    {
        var functionalityId = new FunctionalityId(request.FunctionalityId);
        var functionality = await _functionalityRepository.GetByIdAsync(functionalityId);
        if (functionality is null)
            return Result.Failure<List<OperationDetailedResponse>>(FunctionalityErrors.NotFound(functionalityId));

        var definitions = new[]
        {
            (Name: "View", Suffix: "View"),
            (Name: "Create", Suffix: "Create"),
            (Name: "Update", Suffix: "Update"),
            (Name: "Delete", Suffix: "Delete")
        };

        var existingOperations = new List<(string Name, string Suffix, Operation? Existing)>(definitions.Length);

        foreach (var definition in definitions)
        {
            var code = $"{functionality.Abbreviation}.{definition.Suffix}";
            var friendlyId = $"{functionality.FriendlyId}-{definition.Suffix}";
            var existingByCode = await _operationRepository.GetByCodeAsync(code);
            var existingByFriendlyId = await _operationRepository.GetByFriendlyIdAsync(friendlyId);

            if (existingByCode is not null &&
                existingByFriendlyId is not null &&
                existingByCode.Id == existingByFriendlyId.Id &&
                existingByCode.FunctionalityId == functionality.Id)
            {
                existingOperations.Add((definition.Name, definition.Suffix, existingByCode));
                continue;
            }

            if (existingByCode is not null)
                return Result.Failure<List<OperationDetailedResponse>>(OperationErrors.CodeNotUnique);

            if (existingByFriendlyId is not null)
                return Result.Failure<List<OperationDetailedResponse>>(OperationErrors.FriendlyIdNotUnique);

            existingOperations.Add((definition.Name, definition.Suffix, null));
        }

        var operations = new List<Operation>(definitions.Length);
        var createdAny = false;

        foreach (var (name, suffix, existing) in existingOperations)
        {
            if (existing is not null)
            {
                operations.Add(existing);
                continue;
            }

            var operation = Operation.Create(
                functionality.Id,
                $"{functionality.Name} {name}",
                $"{functionality.Abbreviation}.{suffix}",
                $"{functionality.FriendlyId}-{suffix}");

            _operationRepository.Create(operation);
            operations.Add(operation);
            createdAny = true;
        }

        if (createdAny)
            await _unitOfWork.SaveChangesAsync(cancellationToken);

        return operations
            .Select(operation => operation.ToOperationDetailedResponse())
            .ToList();
    }
}
