using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Modules.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ASystem.Application.UseCases.Modules.Commands.CreateModule;
public sealed class CreateModuleCommand : ICommand<ModuleResponse>
{
	public Guid ApplicationId { get; set; } = Guid.Empty;
	public string Name { get; set; } = string.Empty;
	public string Abbreviation { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public string IconUrl { get; set; } = string.Empty;
	public string FriendlyId { get; set; } = string.Empty;
	public bool IsPartOfMenu { get; set; } = true;
}
