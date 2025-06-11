using _3ASystem.Domain.Abstractions;

namespace _3ASystem.Domain.Entities.Functionalities;

public sealed record FunctionalityCreatedDomainEvent(FunctionalityId FunctionalityId) : IDomainEvent;
