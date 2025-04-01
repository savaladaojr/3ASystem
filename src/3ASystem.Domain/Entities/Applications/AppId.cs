using _3ASystem.Domain.Abstractions;

namespace _3ASystem.Domain.Entities.Applications;

public record AppId : IIdentifier<AppId>
{
	public Guid Value { get; private set; }

	public AppId(Guid value)
	{
		Value = value;
	}

	public AppId GetIdentifier()
	{
		return this;
	}

};
