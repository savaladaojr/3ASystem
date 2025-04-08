using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Entities.Functionalities;
using _3ASystem.Domain.Entities.Roles;
using _3ASystem.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ASystem.Domain.Entities.Modules
{
	public sealed class Module : Entity<ModuleId>
	{
		public AppId ApplicationId { get; private set; } = default!;
		public string Name { get; private set; } = string.Empty;
		public string Abbreviation { get; private set; } = string.Empty;
		public string Description { get; private set; } = string.Empty;
		public string IconUrl { get; private set; } = string.Empty;
		public string FriendlyId { get; private set; } = string.Empty;
		public bool IsActive { get; private set; } = true;
		public bool IsPartOfMenu { get; private set; } = true;


		//EF Relations
		public App? Application { get; init; }
		public ICollection<Functionality>? Functionalities { get; init; }

		private Module()
		{
		}

		private Module(ModuleId id, AppId appId, string name, string abbreviation, string description, string iconUrl, string friendlyId, bool isActive, bool isPartOfMenu) : base(id)
		{
			ApplicationId = appId;
			Name = name;
			Abbreviation = abbreviation;
			Description = description;
			IconUrl = iconUrl;
			FriendlyId = friendlyId;
			IsActive = isActive;
			IsPartOfMenu = isPartOfMenu;

		}

		public static Module Create(AppId appId, string name, string abbreviation, string description, string iconUrl, string friendlyId, bool isPartOfMenu)
		{
			var app = new Module
			(
				new ModuleId(Guid.NewGuid()),
				appId,
				name,
				abbreviation,
				description,
				iconUrl,
				friendlyId,
				true,
				isPartOfMenu
			);

			return app;
		}

		public void Update(string name, string abbreviation, string description, string iconUrl, string friendlyId, bool isPartOfMenu)
		{
			Name = name;
			Abbreviation = abbreviation;
			Description = description;
			IconUrl = iconUrl;
			FriendlyId = friendlyId;
			IsPartOfMenu = isPartOfMenu;
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
