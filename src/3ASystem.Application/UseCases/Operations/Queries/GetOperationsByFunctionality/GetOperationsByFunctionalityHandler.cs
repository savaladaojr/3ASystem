using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Operations.Responses;
using _3ASystem.Domain.Entities.Functionalities;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Operations.Queries.GetOperationsByFunctionality;

public class GetOperationsByFunctionalityHandler : IQueryHandler<GetOperationsByFunctionalityQuery, List<OperationResponse>>
{
    private readonly IOperationRepository _operationRepository;

    public GetOperationsByFunctionalityHandler(IOperationRepository operationRepository)
    {
        _operationRepository = operationRepository;
    }

    public async Task<Result<List<OperationResponse>>> Handle(GetOperationsByFunctionalityQuery request, CancellationToken cancellationToken)
    {
        var functionalityId = new FunctionalityId(request.FunctionalityId);

        var operations = await _operationRepository.GetByFunctionalityIdAsync(functionalityId);

        return operations.Select(o => o.ToOperationResponse()).ToList();
    }
}
