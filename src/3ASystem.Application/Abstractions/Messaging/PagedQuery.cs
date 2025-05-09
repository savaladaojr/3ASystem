using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ASystem.Application.Abstractions.Messaging;

public class PagedQuery
{
	public int PageSize { get; set; } = 25;
	public int Page { get; set; } = 1;

}
