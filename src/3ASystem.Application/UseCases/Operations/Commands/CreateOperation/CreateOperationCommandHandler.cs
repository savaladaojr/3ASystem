using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Operations.Responses;
using _3ASystem.Domain.Entities.Functionalities;
using _3ASystem.Domain.Entities.Operations;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Operations.Commands.CreateOperation;

public class CreateOperationCommandHandler : ICommandHandler<CreateOperationCommand, OperationDetailedResponse>
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

    public async Task<Result<OperationDetailedResponse>> Handle(CreateOperationCommand request, CancellationToken cancellationToken)
    {
        // Check if Code is unique
        var codeExists = await _operationRepository.CodeExistsAsync(request.Code);
        if (codeExists)
            return Result.Failure<OperationDetailedResponse>(OperationErrors.CodeNotUnique);

        // Check if FriendlyId is unique
        var friendlyIdExists = await _operationRepository.FriendlyIdExistsAsync(request.FriendlyId);
        if (friendlyIdExists)
            return Result.Failure<OperationDetailedResponse>(OperationErrors.FriendlyIdNotUnique);

        // Check if Functionality exists
        var functionalityId = new FunctionalityId(request.FunctionalityId);
        var functionality = await _functionalityRepository.GetByIdAsync(functionalityId);
        if (functionality is null)
            return Result.Failure<OperationDetailedResponse>(FunctionalityErrors.NotFound(functionalityId));

        // Create the Operation
        var operation = Operation.Create(
            functionalityId,
            request.Name,
            request.Code,
            request.FriendlyId);

        _operationRepository.Create(operation);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return operation.ToOperationDetailedResponse();
    }
}
