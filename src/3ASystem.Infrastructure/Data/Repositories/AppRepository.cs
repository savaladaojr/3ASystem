using _3ASystem.Domain.Data.Repositories;
using _3ASystem.Domain.Entities.Applications;
using Microsoft.EntityFrameworkCore;

namespace _3ASystem.Infrastructure.Data.Repositories;

public sealed class AppRepository : _Repository<App, AppId>, IAppRepository
{
	public AppRepository(ApplicationDbContext dbContext)
		: base(dbContext)
	{

	}

	public async Task<App?> GetByAbbreviationAsync(string abbreviation)
	{
		var app = await Entity.AsNoTracking().SingleOrDefaultAsync(item => item.Abbreviation == abbreviation);
		return app;
	}

	public async Task<App?> GetByHashAsync(Guid hash)
	{
		var app = await Entity.AsNoTracking().SingleOrDefaultAsync(item => item.Hash == hash);
		return app;
	}

	public async Task<App?> GetByFriendlyIdAsync(string friendlyId)
	{
		var app = await Entity.AsNoTracking().SingleOrDefaultAsync(item => item.FriendlyId == friendlyId);
		return app;
	}

}
