﻿using _3ASystem.Domain.Shared;
using System.Linq.Expressions;

namespace _3ASystem.Application.Abstractions.Data;

public interface IRepository<TEntity>
	where TEntity : Entity
{
	Task<IEnumerable<TEntity>> GetAllAsync();
	Task<IPagedResult<TEntity>> GetAllAsync(int skip, int take);
	Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includePaths);
	Task<TEntity?> GetByIdAsync(object[] keyValues);
	Task<TEntity?> GetByIdAsync(object[] keyValues, params Expression<Func<TEntity, object>>[] includePaths);

	TEntity Create(TEntity entity);
	void Update(TEntity entity);
	void Delete(object[] keyValues);

	void NoTrack(TEntity entity);
}

public interface IRepository<TEntity, TEntityId>
where TEntity : Entity<TEntityId>
where TEntityId : class
{
	Task<IEnumerable<TEntity>> GetAllAsync();
	Task<IPagedResult<TEntity>> GetAllAsync(int skip, int take);
	Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includePaths);
	Task<TEntity?> GetByIdAsync(TEntityId id);
	Task<TEntity?> GetByIdAsync(TEntityId id, params Expression<Func<TEntity, object>>[] includePaths);

	TEntity Create(TEntity entity);
	void Update(TEntity entity);
	void Delete(TEntityId id);


	void NoTrack(TEntity entity);

}
