using _3ASystem.Domain.Data;
using _3ASystem.Domain.Entities;
using _3ASystem.Domain.Entities.Identifiers;

namespace Auth.Domain.Data.Repositories
{
	public interface IOperationRepository : IRepository<Operation, OperationId>
    {
    }
}
