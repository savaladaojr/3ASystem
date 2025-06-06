using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Modules.Responses;

namespace _3ASystem.Application.UseCases.Modules.Commands.UpdateModule;

public sealed class UpdateModuleCommand : ICommand<ModuleDetailedResponse>
{
	public Guid Id { get; set; } = Guid.Empty;
	public Guid ApplicationId { get; set; } = Guid.Empty;
	public string Name { get; set; } = string.Empty;
	public string Abbreviation { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public string IconUrl { get; set; } = string.Empty;
	public string FriendlyId { get; set; } = string.Empty;
	public bool IsPartOfMenu { get; set; } = true;
	public bool IsActive { get; set; } = default!;

}
