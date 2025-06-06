using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Applications.Responses;

namespace _3ASystem.Application.UseCases.Applications.Commands.EnableDisableApplication;

public sealed class EnableDisableApplicationCommand : ICommand<ApplicationDetailedResponse>
{
	public Guid Id { get; set; } = Guid.Empty;

}
