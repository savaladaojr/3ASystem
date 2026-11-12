using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Operations.Responses;

namespace _3ASystem.Application.UseCases.Operations.Queries.GetOperationById;

public sealed class GetOperationByIdQuery : IQuery<OperationDetailedResponse>
{
    public Guid Id { get; set; }
}
