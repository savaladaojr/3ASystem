using _3ASystem.Domain.Abstractions;
using _3ASystem.Domain.Entities.Functionalities;

namespace _3ASystem.Domain.Entities.Applications;

public sealed record FunctionalityCreatedDomainEvent(FunctionalityId AppId) : IDomainEvent;
