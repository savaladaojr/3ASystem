using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Operations.Responses;

namespace _3ASystem.Application.UseCases.Operations.Commands.EnableDisableOperation;

public sealed class EnableDisableOperationCommand : ICommand<OperationDetailedResponse>
{
    public Guid Id { get; set; } = Guid.Empty;
    public bool IsActive { get; set; }
}
