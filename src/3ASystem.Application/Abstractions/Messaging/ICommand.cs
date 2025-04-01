using _3ASystem.Domain.Shared;
using MediatR;

namespace _3ASystem.Application.Abstractions.Messaging;


public interface ICommandBase;

public interface ICommand : IRequest<Result>, ICommandBase;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, ICommandBase;
