using _3ASystem.Application.Abstractions.Messaging;

namespace _3ASystem.Application.UseCases.Operations.Commands.DeleteOperation;

public sealed class DeleteOperationCommand : ICommand
{
    public Guid Id { get; set; } = Guid.Empty;
}
