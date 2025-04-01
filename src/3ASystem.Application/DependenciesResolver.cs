using Application.Abstractions.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace _3ASystem.Application
{
	public static class DependenciesResolver
	{
		public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
		{
			//adding mediatr
			services.AddMediatR(config => {
				config.RegisterServicesFromAssemblies(typeof(DependenciesResolver).Assembly);


				config.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
				config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
			});

			//adding fluent validation
			services.AddValidatorsFromAssembly(typeof(DependenciesResolver).Assembly, includeInternalTypes: true);

			return services;
		}
	}
}
