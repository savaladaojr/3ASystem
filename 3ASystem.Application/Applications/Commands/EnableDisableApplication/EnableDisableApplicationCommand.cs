using _3ASystem.Application.Abstractions.Messaging;

namespace _3ASystem.Application.Applications.Commands.EnableDisableApplication;

public sealed class EnableDisableApplicationCommand : ICommand<EnableDisableApplicationCommandResponse>
{
	public Guid Id { get; set; } = Guid.Empty;

}
