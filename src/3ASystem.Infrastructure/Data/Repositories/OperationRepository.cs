using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Domain.Entities.Operations;

namespace _3ASystem.Infrastructure.Data.Repositories;

public sealed class OperationRepository : _Repository<Operation, OperationId>, IOperationRepository
{
	public OperationRepository(ApplicationDbContext dbContext)
		: base(dbContext)
	{

	}

}
