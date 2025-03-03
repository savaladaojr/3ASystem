using _3ASystem.Domain.Abstractions;
using _3ASystem.Domain.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace _3ASystem.Infrastructure.Data.Repositories
{
	public abstract class _Repository<TEntity> : IRepository<TEntity>
		where TEntity : Entity
	{
		protected readonly ApplicationDbContext _dbContext;

		protected _Repository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public virtual TEntity Create(TEntity entity)
		{
			var createdEntity = _dbContext.Set<TEntity>().Add(entity).Entity;
			return createdEntity;
		}

		public void Update(TEntity entity)
		{
			_dbContext.Set<TEntity>().Update(entity);
		}

		public void Delete(params object[] keyValues)
		{
			var entity = GetById(keyValues).Result;
			if (entity is null) return;

			Delete(entity);
		}

		private void Delete(TEntity entity)
		{
			_dbContext.Set<TEntity>().Remove(entity);
		}

		public virtual async Task<IEnumerable<TEntity>> GetAll()
		{
			return await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();
		}

		public virtual async Task<IEnumerable<TEntity>> GetAll(params Expression<Func<TEntity, object>>[] includePaths)
		{
			var dbSet = _dbContext.Set<TEntity>().AsQueryable();
			var query = includePaths.Aggregate(dbSet, (current, item) => EvaluateInclude(current, item));

			return await query.ToListAsync();
		}

		public virtual async Task<TEntity?> GetById(params object[] keyValues)
		{
			return await _dbContext.Set<TEntity>().FindAsync(keyValues);
		}

		public virtual async Task<TEntity?> GetById(object[] keyValues, params Expression<Func<TEntity, object>>[] includePaths)
		{
			var entity = await GetById(keyValues);

			var dbSet = _dbContext.Set<TEntity>().AsQueryable();
			var query = includePaths.Aggregate(dbSet, (current, item) => EvaluateInclude(current, item));

			return query.Where(item => item == entity).FirstOrDefault();
		}

		//public virtual async Task<List<TEntity>> Serach(Expression<Func<TEntity, bool>> predicate)
		//{
		//    return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
		//}

		//public virtual async Task<PaginatedList<TEntity>> PagedSearch(Expression<Func<TEntity, bool>> predicate, int page, int pageSize)
		//{
		//    return await _dbSet.AsNoTracking().Where(predicate).ToPaginatedListAsync(page, pageSize);
		//}

		public void NoTrack(TEntity entity)
		{
			_dbContext.Entry(entity).State = EntityState.Detached;
		}

		private IQueryable<TEntity> EvaluateInclude(IQueryable<TEntity> current, Expression<Func<TEntity, object>> item)
		{
			if (item.Body is MethodCallExpression)
			{
				var arguments = ((MethodCallExpression)item.Body).Arguments;
				if (arguments.Count > 1)
				{
					var navigationPath = string.Empty;
					for (var i = 0; i < arguments.Count; i++)
					{
						var arg = arguments[i];
						var path = arg.ToString().Substring(arg.ToString().IndexOf('.') + 1);

						navigationPath += (i > 0 ? "." : string.Empty) + path;
					}
					return current.Include(navigationPath);
				}
			}

			return current.Include(item);
		}


	}


	public abstract class _Repository<TEntity, TEntityId> : IRepository<TEntity, TEntityId>
		where TEntity : Entity<TEntityId>
		where TEntityId : class
	{
		protected readonly ApplicationDbContext _dbContext;

		protected _Repository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public virtual async Task<IEnumerable<TEntity>> GetAll()
		{
			return await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();
		}

		public virtual async Task<IEnumerable<TEntity>> GetAll(params Expression<Func<TEntity, object>>[] includePaths)
		{
			var dbSet = _dbContext.Set<TEntity>().AsNoTracking().AsQueryable();
			var query = includePaths.Aggregate(dbSet, (current, item) => EvaluateInclude(current, item));

			return await query.ToListAsync();
		}

		public virtual async Task<TEntity?> GetById(TEntityId id)
		{
			return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(w => w.Id == id);
		}

		public virtual async Task<TEntity?> GetById(TEntityId id, params Expression<Func<TEntity, object>>[] includePaths)
		{
			var dbSet = _dbContext.Set<TEntity>().AsQueryable();
			var query = includePaths.Aggregate(dbSet, (current, item) => EvaluateInclude(current, item));
			query.Where(w => w.Id == id);

			return await query.FirstOrDefaultAsync();
		}

		public TEntity Create(TEntity entity)
		{
			var entityCreated = _dbContext.Set<TEntity>().Add(entity);
			return entityCreated.Entity;
		}

		public void Update(TEntity entity)
		{
			_dbContext.Set<TEntity>().Update(entity);
		}

		public void Delete(TEntityId id)
		{
			var entity = _dbContext.Set<TEntity>().FirstOrDefault(w => w.Id == id);
			if (entity != null)
			{
				_dbContext.Set<TEntity>().Remove(entity);
			}
		}

		public void NoTrack(TEntity entity)
		{
			_dbContext.Entry(entity).State = EntityState.Detached;
		}

		private IQueryable<TEntity> EvaluateInclude(IQueryable<TEntity> current, Expression<Func<TEntity, object>> item)
		{
			if (item.Body is MethodCallExpression)
			{
				var arguments = ((MethodCallExpression)item.Body).Arguments;
				if (arguments.Count > 1)
				{
					var navigationPath = string.Empty;
					for (var i = 0; i < arguments.Count; i++)
					{
						var arg = arguments[i];
						var path = arg.ToString().Substring(arg.ToString().IndexOf('.') + 1);

						navigationPath += (i > 0 ? "." : string.Empty) + path;
					}
					return current.Include(navigationPath);
				}
			}

			return current.Include(item);
		}

	}

}
