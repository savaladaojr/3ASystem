using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Infrastructure.Data;
using _3ASystem.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace _3ASystem.Infrastructure;

public static class DependenciesResolver
{

	public static IServiceCollection AddInfrastructureDependencies(
		this IServiceCollection services, IConfiguration configuration)
	{

		services.AddDbContext<ApplicationDbContext>(options =>
			options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
		);

		services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
		services.AddScoped<IUnitOfWork, UnitOfWork>();

		services.AddScoped<IAppRepository, AppRepository>();
		services.AddScoped<IModuleRepository, ModuleRepository>();
		services.AddScoped<IFunctionalityRepository, FunctionalityRepository>();
		services.AddScoped<IOperationRepository, OperationRepository>();

		return services;
	}
}
