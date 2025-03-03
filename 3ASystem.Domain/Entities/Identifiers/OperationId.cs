using _3ASystem.Domain.Abstractions;

namespace _3ASystem.Domain.Entities.Identifiers
{
	public record OperationId : IIdentifier<OperationId>
	{
		public Guid Value { get; private set; }

		public OperationId(Guid value)
		{
			Value = value;
		}

		public OperationId GetIdentifier()
		{
			return this;
		}
	};

}
