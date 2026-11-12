using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Operations.Responses;
using _3ASystem.Domain.Entities.Operations;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Operations.Commands.UpdateOperation;

public class UpdateOperationCommandHandler : ICommandHandler<UpdateOperationCommand, OperationDetailedResponse>
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

    public async Task<Result<OperationDetailedResponse>> Handle(UpdateOperationCommand request, CancellationToken cancellationToken)
    {
        var operationId = new OperationId(request.Id);

        // Get the operation
        var operation = await _operationRepository.GetByIdAsync(operationId);
        if (operation is null)
            return Result.Failure<OperationDetailedResponse>(OperationErrors.NotFound(operationId));

        // Check if Code is unique (excluding current operation)
        if (operation.Code != request.Code)
        {
            var codeExists = await _operationRepository.CodeExistsAsync(request.Code, operationId);
            if (codeExists)
                return Result.Failure<OperationDetailedResponse>(OperationErrors.CodeNotUnique);
        }

        // Check if FriendlyId is unique (excluding current operation)
        if (operation.FriendlyId != request.FriendlyId)
        {
            var friendlyIdExists = await _operationRepository.FriendlyIdExistsAsync(request.FriendlyId, operationId);
            if (friendlyIdExists)
                return Result.Failure<OperationDetailedResponse>(OperationErrors.FriendlyIdNotUnique);
        }

        // Update the operation
        operation.Update(request.Name, request.Code, request.FriendlyId);
        _operationRepository.Update(operation);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return operation.ToOperationDetailedResponse();
    }
}
