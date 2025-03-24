using _3ASystem.Domain.Abstractions;

namespace _3ASystem.Domain.Entities.Applications;

public sealed record AppDeleteDomainEvent(AppId AppId) : IDomainEvent;