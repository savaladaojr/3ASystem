using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Operations.Responses;
using _3ASystem.Domain.Entities.Operations;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Operations.Queries.GetOperationById;

public class GetOperationByIdQueryHandler : IQueryHandler<GetOperationByIdQuery, OperationDetailedResponse>
{
    private readonly IOperationRepository _operationRepository;

    public GetOperationByIdQueryHandler(IOperationRepository operationRepository)
    {
        _operationRepository = operationRepository;
    }

    public async Task<Result<OperationDetailedResponse>> Handle(GetOperationByIdQuery request, CancellationToken cancellationToken)
    {
        var operationId = new OperationId(request.Id);

        var operation = await _operationRepository.GetByIdWithFunctionalityAsync(operationId);

        if (operation is null)
        {
            return Result.Failure<OperationDetailedResponse>(OperationErrors.NotFound(operationId));
        }

        return operation.ToOperationDetailedResponseWithFunctionality();
    }
}
