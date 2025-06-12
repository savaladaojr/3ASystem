using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Entities.Modules;
using Microsoft.EntityFrameworkCore;

namespace _3ASystem.Infrastructure.Data.Repositories;

public sealed class ModuleRepository : _Repository<Module, ModuleId>, IModuleRepository
{
	public ModuleRepository(ApplicationDbContext dbContext)
		: base(dbContext)
	{

	}

	public async Task<Module?> GetByAbbreviationAsync(string abbreviation)
	{
		var module = await Entity.AsNoTracking().FirstOrDefaultAsync(item => item.Abbreviation == abbreviation);
		return module;
	}

	public async Task<Module?> GetByFriendlyIdAsync(string friendlyId)
	{
		var module = await Entity.AsNoTracking().FirstOrDefaultAsync(item => item.FriendlyId == friendlyId);
		return module;
	}

	public async override Task<IPagedResult<Module>> GetAllAsync(int skip, int take)
	{
		var count = await Entity.AsNoTracking().CountAsync();

		var records = await Entity.AsNoTracking().OrderBy(ord => ord.CreatedAt)
					.Skip(skip).Take(take)
					.Include(m => m.Application)
					.AsSplitQuery() // Use AsSplitQuery to avoid Cartesian product issues with multiple includes
					.ToListAsync();

		var finalResult = new PagedResult<Module>
		{
			TotalOfRecords = count,
			Records = records
		};

		return finalResult;
	}

	public async Task<List<Module>> GetByApplicationIdAsync(AppId appId)
	{
		return await Entity.AsNoTracking().Where(w => w.ApplicationId == appId)	
					.OrderBy(ord => ord.CreatedAt)
					.Include(m => m.Application)
					.AsSplitQuery()
					.ToListAsync();
	}
}
