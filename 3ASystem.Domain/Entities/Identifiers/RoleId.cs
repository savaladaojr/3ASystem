using _3ASystem.Domain.Abstractions;

namespace _3ASystem.Domain.Entities.Identifiers
{
	public record RoleId : IIdentifier<RoleId>
	{
		public Guid Value { get; private set; }

		public RoleId(Guid value)
		{
			Value = value;
		}

		public RoleId GetIdentifier()
		{
			return this;
		}
	};
}
