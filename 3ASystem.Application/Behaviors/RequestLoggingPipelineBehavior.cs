using _3ASystem.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Application.Abstractions.Behaviors;

internal sealed class RequestLoggingPipelineBehavior<TRequest, TResponse>(
	ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> logger)
	: IPipelineBehavior<TRequest, TResponse>
	where TRequest : class
	where TResponse : Result
{
	public async Task<TResponse> Handle(
		TRequest request,
		RequestHandlerDelegate<TResponse> next,
		CancellationToken cancellationToken)
	{
		string requestName = typeof(TRequest).Name;

		logger.LogInformation("Processing request {RequestName}", requestName);

		TResponse result = await next();

		if (result.IsSuccess)
		{
			logger.LogInformation("Completed request {RequestName}", requestName);
		}
		else
		{
			if (result.Error.Type is ErrorType.Validation)
			{
				using (LogContext.PushProperty("Validation", result.Error, true))
				{
					logger.LogWarning("Completed request {RequestName} with validation concerns", requestName);
				}
			}
			else
			{
				using (LogContext.PushProperty("Error", result.Error, true))
				{
					logger.LogError("Completed request {RequestName} with error", requestName);
				}
			}
		}

		return result;
	}
}
