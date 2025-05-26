using _3ASystem.Application.UseCases.Applications.Responses;

namespace _3ASystem.Application.UseCases.Modules.Responses;

public sealed record ModuleCResponse
{
	public Guid Id { get; init; } = default!;
	public Guid ApplicationId { get; init; } = default!;
	public string Name { get; init; } = string.Empty;
	public string Abbreviation { get; init; } = string.Empty;
	public string IconUrl { get; init; } = string.Empty;
	public string FriendlyId { get; init; } = string.Empty;
	public bool IsActive { get; init; } = true;

	public ApplicationCResponse? Application { get; init; }

}
