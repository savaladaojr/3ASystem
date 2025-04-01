using _3ASystem.Domain.Data.Repositories;
using _3ASystem.Domain.Entities.Functionalities;

namespace _3ASystem.Infrastructure.Data.Repositories;

public sealed class FunctionRepository : _Repository<Functionality, FunctionalityId>, IFunctionalityRepository
{
	public FunctionRepository(ApplicationDbContext dbContext)
		: base(dbContext)
	{

	}

}
