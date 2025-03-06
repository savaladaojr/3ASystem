using _3ASystem.Domain.Entities;
using _3ASystem.Domain.Entities.Identifiers;
using Auth.Domain.Data.Repositories;

namespace _3ASystem.Infrastructure.Data.Repositories
{
	internal sealed class OperationRepository : _Repository<Operation, OperationId>, IOperationRepository
	{
		public OperationRepository(ApplicationDbContext dbContext)
			: base(dbContext)
		{

		}

	}
}
