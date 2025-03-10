﻿using _3ASystem.Domain.Abstractions;
using _3ASystem.Domain.Entities.Identifiers;

namespace _3ASystem.Domain.Entities
{
	public sealed class Role : Entity<RoleId>
	{

		public AppId ApplicationId { get; private set; }
		public string Name { get; private set; } = string.Empty;
		public string Code { get; private set; } = string.Empty;
		public bool IsActive { get; private set; } = true;


		//EF Relational
		public App? Application { get; init; }

		//public ICollection<RoleOperation> Operations { get; private set; }


		private Role()
		{

		}

		private Role(RoleId id, AppId applicationId, string name, string code, bool isActive) : base(id)
		{
			ApplicationId = applicationId;
			Name = name;
			Code = code;
			IsActive = isActive;
		}

		public static Role Create(AppId applicationId, string name, string code)
		{
			var role = new Role
			(
				new RoleId(Guid.NewGuid()),
				applicationId,
				name,
				code,
				true
			);

			return role;
		}

		public void Update(string name, string code)
		{
			Name = name;
			Code = code;
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
}
