using _3ASystem.Domain.Abstractions;

namespace _3ASystem.Domain.Entities.Modules;

public sealed record ModuleDeleteDomainEvent(ModuleId AppId) : IDomainEvent;