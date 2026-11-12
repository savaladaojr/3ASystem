using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Operations.Responses;
using _3ASystem.Domain.Entities.Operations;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Operations.Commands.EnableDisableOperation;

public class EnableDisableOperationCommandHandler : ICommandHandler<EnableDisableOperationCommand, OperationDetailedResponse>
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

    public async Task<Result<OperationDetailedResponse>> Handle(EnableDisableOperationCommand request, CancellationToken cancellationToken)
    {
        var operationId = new OperationId(request.Id);

        var operation = await _operationRepository.GetByIdAsync(operationId);
        if (operation is null)
            return Result.Failure<OperationDetailedResponse>(OperationErrors.NotFound(operationId));

        if (request.IsActive)
            operation.Enable();
        else
            operation.Disable();

        _operationRepository.Update(operation);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return operation.ToOperationDetailedResponse();
    }
}
