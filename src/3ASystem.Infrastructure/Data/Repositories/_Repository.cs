using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Numerics;

namespace _3ASystem.Infrastructure.Data.Repositories;

public abstract partial class _Repository<TEntity> : IRepository<TEntity>
	where TEntity : Entity
{
	protected ApplicationDbContext _dbContext;
	protected DbSet<TEntity> Entity { get; init; }

	public _Repository(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
		Entity = _dbContext.Set<TEntity>();
	}

	public virtual TEntity Create(TEntity entity)
	{
		var createdEntity = Entity.Add(entity).Entity;
		return createdEntity;
	}

	public void Update(TEntity entity)
	{
		Entity.Update(entity);
	}

	public void Delete(params object[] keyValues)
	{
		var entity = GetByIdAsync(keyValues).Result;
		if (entity is null) return;

		Delete(entity);
	}

	private void Delete(TEntity entity)
	{
		Entity.Remove(entity);
	}

	public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
	{
		return await Entity.AsNoTracking().ToListAsync();
	}

	public virtual async Task<IPagedResult<TEntity>> GetAllAsync(int skip, int take)
	{
		var count = await Entity.AsNoTracking().CountAsync();
		var records = await Entity.AsNoTracking().Skip(skip).Take(take).ToListAsync();

		var finalResult = new PagedResult<TEntity>
		{
			TotalOfRecords = count,
			Records = records
		};

		return finalResult;
	}

	public virtual async Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includePaths)
	{
		var dbSet = Entity.AsQueryable();
		var query = includePaths.Aggregate(dbSet, (current, item) => EvaluateInclude(current, item));

		return await query.ToListAsync();
	}

	public virtual async Task<TEntity?> GetByIdAsync(params object[] keyValues)
	{
		return await Entity.FindAsync(keyValues);
	}

	public virtual async Task<TEntity?> GetByIdAsync(object[] keyValues, params Expression<Func<TEntity, object>>[] includePaths)
	{
		var entity = await GetByIdAsync(keyValues);

		var dbSet = Entity.AsQueryable();
		var query = includePaths.Aggregate(dbSet, (current, item) => EvaluateInclude(current, item));

		return query.Where(item => item == entity).FirstOrDefault();
	}

	//public virtual async Task<List<TEntity>> Serach(Expression<Func<TEntity, bool>> predicate)
	//{
	//    return await Entity.AsNoTracking().Where(predicate).ToListAsync();
	//}

	//public virtual async Task<PaginatedList<TEntity>> PagedSearch(Expression<Func<TEntity, bool>> predicate, int page, int pageSize)
	//{
	//    return await Entity.AsNoTracking().Where(predicate).ToPaginatedListAsync(page, pageSize);
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
	protected DbSet<TEntity> Entity { get; init; }

	public _Repository(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
		Entity = _dbContext.Set<TEntity>();
	}

	public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
	{
		return await Entity.AsNoTracking().ToListAsync();
	}

	public virtual async Task<IPagedResult<TEntity>> GetAllAsync(int skip, int take)
	{
		var count = await Entity.AsNoTracking().CountAsync();

		var records = await Entity.AsNoTracking().OrderBy(ord => ord.CreatedAt).Skip(skip).Take(take).ToListAsync();

		var finalResult = new PagedResult<TEntity>
		{
			TotalOfRecords = count,
			Records = records
		};

		return finalResult;
	}

	public virtual async Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includePaths)
	{
		var dbSet = Entity.AsNoTracking().AsQueryable();
		var query = includePaths.Aggregate(dbSet, (current, item) => EvaluateInclude(current, item));

		return await query.ToListAsync();
	}

	public virtual async Task<TEntity?> GetByIdAsync(TEntityId id)
	{
		return await Entity.FirstOrDefaultAsync(w => w.Id == id);
	}

	public virtual async Task<TEntity?> GetByIdAsync(TEntityId id, params Expression<Func<TEntity, object>>[] includePaths)
	{
		var dbSet = Entity.AsQueryable();
		var query = includePaths.Aggregate(dbSet, (current, item) => EvaluateInclude(current, item));
		query.Where(w => w.Id == id);

		return await query.FirstOrDefaultAsync();
	}

	public TEntity Create(TEntity entity)
	{
		var entityCreated = Entity.Add(entity);
		return entityCreated.Entity;
	}

	public void Update(TEntity entity)
	{
		Entity.Update(entity);
	}

	public void Delete(TEntityId id)
	{
		var entity = _dbContext.Set<TEntity>().FirstOrDefault(w => w.Id == id);
		if (entity != null)
		{
			Entity.Remove(entity);
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
