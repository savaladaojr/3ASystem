using _3ASystem.Domain.Abstractions;

namespace _3ASystem.Domain.Entities.Modules;

public record ModuleId : IIdentifier<ModuleId>
{
	public Guid Value { get; private set; }

	public ModuleId(Guid value)
	{
		Value = value;
	}

	public ModuleId GetIdentifier()
	{
		return this;
	}

};