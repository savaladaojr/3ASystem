namespace _3ASystem.Domain.Abstractions
{
	public abstract class Entity : IEntity
	{
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public DateTime LastUpdatedAt { get; set; } = DateTime.Now;
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

}
