using _3ASystem.Domain.Abstractions;
using System.Linq.Expressions;

namespace _3ASystem.Domain.Data
{
	public interface IRepository<TEntity>
	where TEntity : Entity
	{
		Task<IEnumerable<TEntity>> GetAll();
		Task<IEnumerable<TEntity>> GetAll(params Expression<Func<TEntity, object>>[] includePaths);
		Task<TEntity?> GetById(object[] keyValues);
		Task<TEntity?> GetById(object[] keyValues, params Expression<Func<TEntity, object>>[] includePaths);

		TEntity Create(TEntity entity);
		void Update(TEntity entity);
		void Delete(object[] keyValues);

		void NoTrack(TEntity entity);
	}

	public interface IRepository<TEntity, TEntityId>
	where TEntity : Entity<TEntityId>
	where TEntityId : class
	{
		Task<IEnumerable<TEntity>> GetAll();
		Task<IEnumerable<TEntity>> GetAll(params Expression<Func<TEntity, object>>[] includePaths);
		Task<TEntity?> GetById(TEntityId id);
		Task<TEntity?> GetById(TEntityId id, params Expression<Func<TEntity, object>>[] includePaths);

		TEntity Create(TEntity entity);
		void Update(TEntity entity);
		void Delete(TEntityId id);


		void NoTrack(TEntity entity);

	}

}
