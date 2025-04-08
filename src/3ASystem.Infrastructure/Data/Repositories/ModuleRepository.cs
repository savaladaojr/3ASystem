using _3ASystem.Domain.Data.Repositories;
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

	public async Task<Module?> GetByFriendlyIdAsync(string friendlyId)
	{
		var module = await Entity.AsNoTracking().FirstOrDefaultAsync(item => item.FriendlyId == friendlyId);
		return module;
	}
}
