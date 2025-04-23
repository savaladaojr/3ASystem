using _3ASystem.Domain.Data;
using _3ASystem.Domain.Entities.Applications;

namespace _3ASystem.Domain.Data.Repositories
{
	public interface IAppRepository : IRepository<App, AppId>
	{
		Task<App?> GetByAbbreviationAsync(string abbreviation);
		Task<App?> GetByFriendlyIdAsync(string friendlyId);
		Task<App?> GetByHashAsync(Guid hash);
	}
}
