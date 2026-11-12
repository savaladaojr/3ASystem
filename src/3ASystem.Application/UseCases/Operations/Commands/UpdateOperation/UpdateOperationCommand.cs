using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Operations.Responses;

namespace _3ASystem.Application.UseCases.Operations.Commands.UpdateOperation;

public sealed class UpdateOperationCommand : ICommand<OperationDetailedResponse>
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string FriendlyId { get; set; } = string.Empty;
}
