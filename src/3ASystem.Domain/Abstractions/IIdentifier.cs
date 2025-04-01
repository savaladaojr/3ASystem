namespace _3ASystem.Domain.Abstractions
{
	public interface IIdentifier
	{
		object[] GetIdentifier();
	}

	public interface IIdentifier<TEntityId> where TEntityId : class
	{
		TEntityId GetIdentifier();
	}
}
