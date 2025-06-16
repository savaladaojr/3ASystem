using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Applications.Responses;
using _3ASystem.Application.UseCases.Functionalities.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ASystem.Application.UseCases.Functionalities.Commands.UpdateFunctionality;
public sealed class UpdateFunctionalityCommand : ICommand<FunctionalityDetailedResponse>
{
	public Guid Id { get; set; } = default!;
	public Guid ModuleId { get; set; } = Guid.Empty;
	public string Name { get; set; } = string.Empty;
	public string Abbreviation { get; set; } = string.Empty;
	public string Route { get; set; } = string.Empty;
	public string IconUrl { get; set; } = string.Empty;
	public string FriendlyId { get; set; } = string.Empty;
	public bool IsPartOfMenu { get; set; } = true;
}
