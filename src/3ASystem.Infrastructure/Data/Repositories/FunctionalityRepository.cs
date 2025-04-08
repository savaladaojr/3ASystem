using _3ASystem.Domain.Data.Repositories;
using _3ASystem.Domain.Entities.Functionalities;

namespace _3ASystem.Infrastructure.Data.Repositories;

public sealed class FunctionalityRepository : _Repository<Functionality, FunctionalityId>, IFunctionalityRepository
{
	public FunctionalityRepository(ApplicationDbContext dbContext)
		: base(dbContext)
	{

	}

}
