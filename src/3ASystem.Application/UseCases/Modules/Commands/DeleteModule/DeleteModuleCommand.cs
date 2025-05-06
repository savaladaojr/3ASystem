using _3ASystem.Application.Abstractions.Messaging;

namespace _3ASystem.Application.UseCases.Modules.Commands.DeleteModule;

public sealed record DeleteModuleCommand(Guid Id) : ICommand;
