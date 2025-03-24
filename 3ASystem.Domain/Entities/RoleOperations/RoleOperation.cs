using _3ASystem.Domain.Entities.Operations;
using _3ASystem.Domain.Entities.Roles;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Domain.Entities.RoleOperations;

public sealed class RoleOperation : Entity
{
	public RoleId RoleId { get; private set; } = default!;
	public OperationId OperationId { get; private set; } = default!;
	public bool IsAllowed { get; private set; }


	//EF Relation
	public Role? Role { get; init; }
	public Operation? Operation { get; init; }


	private RoleOperation()
	{
	}

	private RoleOperation(RoleId roleId, OperationId operationId, bool isAllowed)
	{
		RoleId = roleId;
		OperationId = operationId;
		IsAllowed = isAllowed;
	}

	public static RoleOperation Create(RoleId roleId, OperationId operationId)
	{
		var roleOperation = new RoleOperation
		(
			roleId,
			operationId,
			true
		);

		return roleOperation;
	}

	public void Allow()
	{
		IsAllowed = true;
		LastUpdatedAt = DateTime.Now;
	}

	public void Forbid()
	{
		IsAllowed = false;
		LastUpdatedAt = DateTime.Now;
	}
}
