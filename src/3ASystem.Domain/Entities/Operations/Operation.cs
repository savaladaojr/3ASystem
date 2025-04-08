using _3ASystem.Domain.Entities.Functionalities;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Domain.Entities.Operations;

public sealed class Operation : Entity<OperationId>
{
	public FunctionalityId FunctionalityId { get; private set; } = default!;

	public string Name { get; private set; } = default!;
	public string Code { get; private set; } = default!;
	public bool IsActive { get; private set; } = true;
	public string FriendlyId { get; private set; } = default!;


	//EF Relations
	public Functionality? Functionality { get; init; }

	private Operation()
	{
	}

	private Operation(OperationId id, FunctionalityId functionalityId, string name, string code, bool isActive, string friendlyId) : base(id)
	{
		FunctionalityId = functionalityId;
		Name = name;
		Code = code;
		IsActive = isActive;
		FriendlyId = friendlyId;
	}

	public static Operation Create(FunctionalityId functionalityId, string name, string code, string friendlyId)
	{
		var action = new Operation
		(
			new OperationId(Guid.NewGuid()),
			functionalityId,
			name,
			code,
			true,
			friendlyId
		);

		return action;
	}

	public void Update(string name, string code, string friendlyId)
	{
		Name = name;
		Code = code;
		FriendlyId = friendlyId;
		LastUpdatedAt = DateTime.Now;
	}

	public void Enable()
	{
		IsActive = true;
		LastUpdatedAt = DateTime.Now;
	}

	public void Disable()
	{
		IsActive = false;
		LastUpdatedAt = DateTime.Now;
	}


}
