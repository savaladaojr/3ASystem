using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.Applications.Shared;

namespace _3ASystem.Application.Applications.Commands.EnableDisableApplication;

public sealed class EnableDisableApplicationCommand : ICommand<ApplicationResponse>
{
	public Guid Id { get; set; } = Guid.Empty;

}
