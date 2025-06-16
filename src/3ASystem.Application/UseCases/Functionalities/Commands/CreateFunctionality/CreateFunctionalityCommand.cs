using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Functionalities.Responses;

namespace _3ASystem.Application.UseCases.Functionalities.Commands.CreateFunctionality;
public sealed class CreateFunctionalityCommand : ICommand<FunctionalityDetailedResponse>
{
	public Guid ModuleId { get; set; } = Guid.Empty;
	public string Name { get; set; } = string.Empty;
	public string Abbreviation { get; set; } = string.Empty;
	public string Route { get; set; } = string.Empty;
	public string IconUrl { get; set; } = string.Empty;
	public string FriendlyId { get; set; } = string.Empty;
	public bool IsPartOfMenu { get; set; } = true;
}
