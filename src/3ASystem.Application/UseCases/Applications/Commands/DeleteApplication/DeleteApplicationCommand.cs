using _3ASystem.Application.Abstractions.Messaging;

namespace _3ASystem.Application.UseCases.Applications.Commands.DeleteApplication;

public sealed record DeleteApplicationCommand(Guid Id) : ICommand;
