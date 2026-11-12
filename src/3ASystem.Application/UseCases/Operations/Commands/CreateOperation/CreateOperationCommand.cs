using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Operations.Responses;

namespace _3ASystem.Application.UseCases.Operations.Commands.CreateOperation;

public sealed class CreateOperationCommand : ICommand<OperationDetailedResponse>
{
    public Guid FunctionalityId { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string FriendlyId { get; set; } = string.Empty;
}
