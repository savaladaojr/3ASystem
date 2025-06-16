using _3ASystem.Application.Abstractions.Messaging;

namespace _3ASystem.Application.UseCases.Functionalities.Commands.DeleteFunctionality;

public sealed record DeleteFunctionalityCommand(Guid Id) : ICommand;
