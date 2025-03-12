using _3ASystem.Domain.Shared;
using MediatR;

namespace _3ASystem.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}