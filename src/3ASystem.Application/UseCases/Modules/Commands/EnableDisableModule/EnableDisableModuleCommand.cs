using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Modules.Responses;

namespace _3ASystem.Application.UseCases.Modules.Commands.EnableDisableModule;

public sealed class EnableDisableModuleCommand : ICommand<ModuleResponse>
{
	public Guid Id { get; set; } = Guid.Empty;

}
