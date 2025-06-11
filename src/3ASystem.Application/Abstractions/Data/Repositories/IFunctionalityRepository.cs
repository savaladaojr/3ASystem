using _3ASystem.Domain.Entities.Functionalities;

namespace _3ASystem.Application.Abstractions.Data.Repositories;

public interface IFunctionalityRepository : IRepository<Functionality, FunctionalityId>
{
	public Task<Functionality?> GetByAbbreviationAsync(string abbreviation);
	public Task<Functionality?> GetByFriendlyIdAsync(string friendlyId);
}
