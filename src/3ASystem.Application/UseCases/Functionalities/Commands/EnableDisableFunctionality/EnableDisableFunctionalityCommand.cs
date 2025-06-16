using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Functionalities.Responses;

namespace _3ASystem.Application.UseCases.Functionalities.Commands.EnableDisableFunctionality;

public sealed class EnableDisableFunctionalityCommand : ICommand<FunctionalityDetailedResponse>
{
	public Guid Id { get; set; } = Guid.Empty;

}
