using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Infrastructure.Data;


public class PagedResult<TEntity> : IPagedResult<TEntity>
{
	public int TotalOfRecords { get; set; }
	public IList<TEntity> Records { get; set; } = new List<TEntity>();
}

