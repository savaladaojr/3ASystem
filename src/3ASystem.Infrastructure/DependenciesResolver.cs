using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Services;
using _3ASystem.Infrastructure.Data;
using _3ASystem.Infrastructure.Data.Repositories;
using _3ASystem.Infrastructure.Services.EmailService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace _3ASystem.Infrastructure;

public static class DependenciesResolver
{

	public static IServiceCollection AddInfrastructureDependencies(
		this IServiceCollection services, IConfiguration configuration)
	{
		services.AddMemoryCache();

		var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
		services.AddDbContext<ApplicationDbContext>(options =>
			options.UseSqlServer(connectionString)
		);

		services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
		services.AddScoped<IUnitOfWork, UnitOfWork>();

		services.AddScoped<IAppRepository, AppRepository>();
		services.AddScoped<IModuleRepository, ModuleRepository>();
		services.AddScoped<IFunctionalityRepository, FunctionalityRepository>();
		services.AddScoped<IOperationRepository, OperationRepository>();


		//Email Service
		services.AddSingleton<IEmailService, EmailService>();



		return services;
	}
}
