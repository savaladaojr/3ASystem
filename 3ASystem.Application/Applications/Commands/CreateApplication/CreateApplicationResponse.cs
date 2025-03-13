namespace _3ASystem.Application.Applications.Commands.CreateApplication;

public sealed class CreateApplicationResponse
{
	public Guid Id { get; init; }
	public string Name { get; init; } = string.Empty;
	public string Abbreviation { get; init; } = string.Empty;
	public string Description { get; init; } = string.Empty;
	public string IconUrl { get; init; } = string.Empty;
	public Guid Hash { get; init; } = default!;
	public bool IsActive { get; init; } = default!;

}
