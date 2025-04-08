using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Domain.Entities.Roles;

public sealed class Role : Entity<RoleId>
{

	public AppId ApplicationId { get; private set; } = default!;
	public string Name { get; private set; } = string.Empty;
	public string Code { get; private set; } = string.Empty;
	public bool IsActive { get; private set; } = true;
	public string FriendlyId { get; private set; } = default!;

	//EF Relational
	public App? Application { get; init; }

	//public ICollection<RoleOperation> Operations { get; private set; }


	private Role()
	{

	}

	private Role(RoleId id, AppId applicationId, string name, string code, bool isActive, string friendlyId) : base(id)
	{
		ApplicationId = applicationId;
		Name = name;
		Code = code;
		IsActive = isActive;
		FriendlyId = friendlyId;
	}

	public static Role Create(AppId applicationId, string name, string code, string friendlyId)
	{
		var role = new Role
		(
			new RoleId(Guid.NewGuid()),
			applicationId,
			name,
			code,
			true,
			friendlyId
		);

		return role;
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
