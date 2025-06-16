using _3ASystem.Application.UseCases.Modules.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ASystem.Application.UseCases.Functionalities.Responses;
public sealed class FunctionalityResponse
{
	public Guid Id { get; init; } = default!;
	public Guid ModuleId { get; init; } = default!;
	public string Name { get; init; } = default!;
	public string Abbreviation { get; init; } = default!;
	public string IconUrl { get; init; } = string.Empty;
	public string FriendlyId { get; init; } = string.Empty;
	public bool IsActive { get; init; } = true;

	public ModuleResponse? Module { get; init; }

}
