using _3ASystem.Domain.Entities.Modules;

namespace _3ASystem.Application.Abstractions.Data.Repositories
{
	public interface IModuleRepository : IRepository<Module, ModuleId>
	{
		Task<Module?> GetByFriendlyIdAsync(string friendlyId);

		Task<Module?> GetByAbbreviationAsync(string abbreviation);

	}
}
