using _3ASystem.Application.Abstractions.Data;
using Microsoft.EntityFrameworkCore;

namespace _3ASystem.Infrastructure.Data;

public sealed class UnitOfWork : DbContext, IUnitOfWork
{
	private ApplicationDbContext _dbContext;

	public UnitOfWork(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		var result = await _dbContext.SaveChangesAsync(cancellationToken);
		_dbContext.ChangeTracker.Clear(); // Clear the change tracker to avoid memory leaks and stale data

		return result;
	}

	//This way it is not necessary include the async in the method signature and also the return of the SaveChangesAsync
	//public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => _dbContext.SaveChangesAsync(cancellationToken);
}
