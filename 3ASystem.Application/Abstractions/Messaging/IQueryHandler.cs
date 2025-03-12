using _3ASystem.Domain.Shared;
using MediatR;

namespace _3ASystem.Application.Abstractions.Messaging;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
	where TQuery : IQuery<TResponse>
{
}