using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Domain.Entities.Operations;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Operations.Commands.DeleteOperation;

public class DeleteOperationCommandHandler : ICommandHandler<DeleteOperationCommand>
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

    public async Task<Result> Handle(DeleteOperationCommand request, CancellationToken cancellationToken)
    {
        var operationId = new OperationId(request.Id);

        var operation = await _operationRepository.GetByIdAsync(operationId);
        if (operation is null)
            return Result.Failure(OperationErrors.NotFound(operationId));

        _operationRepository.Delete(operationId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
