using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Domain.Entities.Functionalities;
using _3ASystem.Domain.Entities.Operations;
using Microsoft.EntityFrameworkCore;

namespace _3ASystem.Infrastructure.Data.Repositories;

public sealed class OperationRepository : _Repository<Operation, OperationId>, IOperationRepository
{
	public OperationRepository(ApplicationDbContext dbContext)
		: base(dbContext)
	{

	}

	public async Task<IEnumerable<Operation>> GetByFunctionalityIdAsync(FunctionalityId functionalityId)
	{
		return await _dbContext.Set<Operation>()
			.AsNoTracking()
			.Where(o => o.FunctionalityId == functionalityId)
			.OrderBy(o => o.Name)
			.ToListAsync();
	}

	public async Task<bool> ExistsByFunctionalityIdAsync(FunctionalityId functionalityId)
	{
		return await _dbContext.Set<Operation>()
			.AnyAsync(o => o.FunctionalityId == functionalityId);
	}

	public async Task<Operation?> GetByIdWithFunctionalityAsync(OperationId operationId)
	{
		return await Entity
			.AsNoTracking()
			.Include(operation => operation.Functionality)
			.ThenInclude(functionality => functionality!.Module)
			.FirstOrDefaultAsync(operation => operation.Id == operationId);
	}

	public async Task<Operation?> GetByCodeAsync(string code)
	{
		return await _dbContext.Set<Operation>()
			.AsNoTracking()
			.FirstOrDefaultAsync(o => o.Code == code);
	}

	public async Task<Operation?> GetByFriendlyIdAsync(string friendlyId)
	{
		return await _dbContext.Set<Operation>()
			.AsNoTracking()
			.FirstOrDefaultAsync(o => o.FriendlyId == friendlyId);
	}

	public async Task<bool> CodeExistsAsync(string code, OperationId? excludeId = null)
	{
		var query = _dbContext.Set<Operation>()
			.Where(o => o.Code == code);

		if (excludeId is not null)
			query = query.Where(o => o.Id != excludeId);

		return await query.AnyAsync();
	}

	public async Task<bool> FriendlyIdExistsAsync(string friendlyId, OperationId? excludeId = null)
	{
		var query = _dbContext.Set<Operation>()
			.Where(o => o.FriendlyId == friendlyId);

		if (excludeId is not null)
			query = query.Where(o => o.Id != excludeId);

		return await query.AnyAsync();
	}
}
