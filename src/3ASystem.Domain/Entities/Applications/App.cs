using _3ASystem.Domain.Entities.Modules;
using _3ASystem.Domain.Entities.Roles;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Domain.Entities.Applications;

public sealed class App : Entity<AppId>
{
	public string Name { get; private set; } = string.Empty;
	public string Abbreviation { get; private set; } = string.Empty;
	public string Description { get; private set; } = string.Empty;
	public string IconUrl { get; private set; } = string.Empty;
	public Guid Hash { get; private set; }
	public string FriendlyId { get; private set; } = default!;
	public bool IsActive { get; private set; } = true;


	//EF Relations
	public ICollection<Module>? Modules { get; init; }

	public ICollection<Role>? Roles { get; init; }
	

	private App()
	{
	}

	private App(AppId id, string name, string abbreviation, string description, string iconUrl, Guid hash, string friendlyId, bool isActive) : base(id)
	{
		Name = name;
		Abbreviation = abbreviation;
		Description = description;
		IconUrl = iconUrl;
		Hash = hash;
		FriendlyId = friendlyId;
		IsActive = isActive;

		//Modules = new List<Module>();
		//Roles = new List<Role>();
	}

	public static App Create(string name, string abbreviation, string description, string iconUrl, string friendlyId)
	{
		var app = new App
		(
			new AppId(Guid.NewGuid()),
			name,
			abbreviation,
			description,
			iconUrl,
			Guid.NewGuid(),
			friendlyId,
			true
		);

		return app;
	}

	public void Update(string name, string abbreviation, string description, string iconUrl, string friendlyId)
	{
		Name = name;
		Abbreviation = abbreviation;
		Description = description;
		IconUrl = iconUrl;
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
