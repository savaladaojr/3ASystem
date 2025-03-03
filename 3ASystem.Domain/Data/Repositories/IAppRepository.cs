using _3ASystem.Domain.Data;
using _3ASystem.Domain.Entities;
using _3ASystem.Domain.Entities.Identifiers;

namespace _3ASystem.Domain.Data.Repositories
{
	public interface IAppRepository : IRepository<App, AppId>
	{
	}
}
