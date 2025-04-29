namespace _3ASystem.Application.UseCases.Modules.Responses;

public sealed class ModuleResponse
{
	public Guid Id { get; init; } = default!;
	public Guid ApplicationId { get; init; } = default!;
	public string Name { get; init; } = string.Empty;
	public string Abbreviation { get; init; } = string.Empty;
	public string Description { get; init; } = string.Empty;
	public string IconUrl { get; init; } = string.Empty;
	public string FriendlyId { get; init; } = string.Empty;
	public bool IsActive { get; init; } = true;
	public bool IsPartOfMenu { get; init; } = true;

	public DateTime CreatedAt { get; init; } = default!;
	public DateTime UpdatedAt { get; init; } = default!;

}
