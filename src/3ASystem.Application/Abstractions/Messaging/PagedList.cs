using _3ASystem.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ASystem.Application.Abstractions.Messaging;
public class PagedList<TValue>
{
	public int ActualPage { get; set; }
	public int TotalOfRecords { get; set; }
	public int TotalOfRecordsPerPage { get; set; } //PageSize
	public int TotalOfPages { get => (int)Math.Ceiling((double)this.TotalOfRecords / this.TotalOfRecordsPerPage); }

	public List<TValue> Records { get; set; } = default!;
}
