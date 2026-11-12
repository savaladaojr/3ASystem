namespace _3ASystem.Application.UseCases.Operations.Responses;

public sealed class OperationResponse
{
    public Guid Id { get; init; } = default!;
    public Guid FunctionalityId { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string Code { get; init; } = default!;
    public string FriendlyId { get; init; } = default!;
    public bool IsActive { get; init; } = true;
    public DateTime CreatedAt { get; init; } = default!;
    public DateTime LastUpdatedAt { get; init; } = default!;
}
