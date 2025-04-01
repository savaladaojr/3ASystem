using _3ASystem.Domain.Entities.Operations;
using Auth.Domain.Data.Repositories;

namespace _3ASystem.Infrastructure.Data.Repositories;

public sealed class OperationRepository : _Repository<Operation, OperationId>, IOperationRepository
{
	public OperationRepository(ApplicationDbContext dbContext)
		: base(dbContext)
	{

	}

}
