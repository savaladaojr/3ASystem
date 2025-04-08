using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ASystem.Application.Applications.Queries.GetApplications;

public sealed record ApplicationResponse
{
	public Guid Id { get; init; }
	public string Name { get; init; } = string.Empty;
	public string Abbreviation { get; init; } = string.Empty;
	public string IconUrl { get; init; } = string.Empty;
	public bool IsActive { get; init; } = true;
	public string FriendlyId { get; init; } = string.Empty;

}
