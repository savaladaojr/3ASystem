using _3ASystem.Application.Abstractions.Messaging;

namespace _3ASystem.Application.Applications.Commands.DeleteApplication;

public sealed record DeleteApplicationsCommand(Guid Id) : ICommand;
