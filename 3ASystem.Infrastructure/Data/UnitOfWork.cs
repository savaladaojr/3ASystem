using _3ASystem.Domain.Data;

namespace _3ASystem.Infrastructure.Data
{
	public sealed class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _dbContext;

		public UnitOfWork(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return await _dbContext.SaveChangesAsync(cancellationToken);
		}

		//This way it is not necessary include the async in the method signature and also the return of the SaveChangesAsync
		//public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => _dbContext.SaveChangesAsync(cancellationToken);
	}
}
