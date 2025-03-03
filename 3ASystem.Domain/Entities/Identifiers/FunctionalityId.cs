using _3ASystem.Domain.Abstractions;

namespace _3ASystem.Domain.Entities.Identifiers
{
	public record FunctionalityId : IIdentifier<FunctionalityId>
	{
		public Guid Value { get; private set; }

		public FunctionalityId(Guid value)
		{
			Value = value;
		}

		public FunctionalityId GetIdentifier()
		{
			return this;
		}
	};
}
