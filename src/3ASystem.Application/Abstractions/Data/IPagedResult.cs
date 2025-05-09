using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ASystem.Application.Abstractions.Data;
public interface IPagedResult<TEntity>
{
	public int TotalOfRecords { get; set; }
	public IList<TEntity> Records { get; set; }
}
