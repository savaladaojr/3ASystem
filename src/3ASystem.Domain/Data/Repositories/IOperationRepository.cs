using _3ASystem.Domain.Data;
using _3ASystem.Domain.Entities.Operations;

namespace Auth.Domain.Data.Repositories
{
	public interface IOperationRepository : IRepository<Operation, OperationId>
    {
    }
}
