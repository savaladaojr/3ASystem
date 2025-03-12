using _3ASystem.Domain.Abstractions;

namespace _3ASystem.Domain.Shared;

public abstract class Entity : IEntity
{
	private readonly List<IDomainEvent> _domainEvents = [];


	public DateTime CreatedAt { get; set; } = DateTime.Now;
	public DateTime LastUpdatedAt { get; set; } = DateTime.Now;



	public List<IDomainEvent> DomainEvents => [.. _domainEvents];

	public void ClearDomainEvents()
	{
		_domainEvents.Clear();
	}

	public void Raise(IDomainEvent domainEvent)
	{
		_domainEvents.Add(domainEvent);
	}
}


public abstract class Entity<TEntityId> : Entity, IEntity
{
	protected Entity(TEntityId id)
	{
		Id = id;
	}

	protected Entity()
	{

	}

	public TEntityId Id { get; init; } = default!;

}
