using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Operations.Responses;

namespace _3ASystem.Application.UseCases.Operations.Queries.GetOperationsByFunctionality;

public sealed class GetOperationsByFunctionalityQuery : IQuery<List<OperationResponse>>
{
    public Guid FunctionalityId { get; set; }
}
