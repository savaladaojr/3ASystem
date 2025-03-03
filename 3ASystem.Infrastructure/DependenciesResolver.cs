using _3ASystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace _3ASystem.Infrastructure
{
	public static class DependenciesResolver
	{

		public static IServiceCollection AddInfrastructureDependencies(
			this IServiceCollection services, IConfiguration configuration)
		{

			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
			);

			return services;
		}
	}
}
