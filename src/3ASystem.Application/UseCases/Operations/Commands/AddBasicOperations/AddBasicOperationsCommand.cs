using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Operations.Responses;

namespace _3ASystem.Application.UseCases.Operations.Commands.AddBasicOperations;

public sealed class AddBasicOperationsCommand : ICommand<List<OperationDetailedResponse>>
{
    public Guid FunctionalityId { get; set; } = Guid.Empty;
}
