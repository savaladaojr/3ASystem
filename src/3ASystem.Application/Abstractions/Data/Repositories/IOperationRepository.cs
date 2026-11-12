using _3ASystem.Domain.Entities.Functionalities;
using _3ASystem.Domain.Entities.Operations;

namespace _3ASystem.Application.Abstractions.Data.Repositories
{
	public interface IOperationRepository : IRepository<Operation, OperationId>
	{
		Task<IEnumerable<Operation>> GetByFunctionalityIdAsync(FunctionalityId functionalityId);
		Task<bool> ExistsByFunctionalityIdAsync(FunctionalityId functionalityId);
		Task<Operation?> GetByIdWithFunctionalityAsync(OperationId operationId);
		Task<Operation?> GetByCodeAsync(string code);
		Task<Operation?> GetByFriendlyIdAsync(string friendlyId);
		Task<bool> CodeExistsAsync(string code, OperationId? excludeId = null);
		Task<bool> FriendlyIdExistsAsync(string friendlyId, OperationId? excludeId = null);
	}
}
