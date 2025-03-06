using _3ASystem.Domain.Data.Repositories;
using _3ASystem.Domain.Entities;
using _3ASystem.Domain.Entities.Identifiers;

namespace _3ASystem.Infrastructure.Data.Repositories
{
	internal sealed class AppRepository : _Repository<App, AppId>, IAppRepository
	{
		public AppRepository(ApplicationDbContext dbContext)
			: base(dbContext)
		{

		}

	}
}
