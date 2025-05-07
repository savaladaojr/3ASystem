using _3ASystem.Domain.Entities.Applications;

namespace _3ASystem.Application.Abstractions.Data.Repositories;

public interface IAppRepository : IRepository<App, AppId>
{
	Task<App?> GetByAbbreviationAsync(string abbreviation);
	Task<App?> GetByFriendlyIdAsync(string friendlyId);
	Task<App?> GetByHashAsync(Guid hash);
}
