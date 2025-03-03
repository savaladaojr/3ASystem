using _3ASystem.Domain.Data.Repositories;
using _3ASystem.Domain.Entities;
using _3ASystem.Domain.Entities.Identifiers;

namespace _3ASystem.Infrastructure.Data.Repositories
{
	internal sealed class FunctionRepository : _Repository<Functionality, FunctionalityId>, IFunctionalityRepository
	{
		public FunctionRepository(ApplicationDbContext dbContext)
			: base(dbContext)
		{

		}

	}
}
