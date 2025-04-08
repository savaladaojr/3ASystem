using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Entities.Modules;
using _3ASystem.Domain.Entities.Operations;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Domain.Entities.Functionalities;

public sealed class Functionality : Entity<FunctionalityId>
{
	public ModuleId ModuleId { get; private set; } = default!;
	public string Name { get; private set; } = default!;
	public string Abbreviation { get; private set; } = default!;
	public string Route { get; private set; } = default!;
	public string FriendlyId { get; private set; } = default!;
	public bool IsActive { get; set; } = true;
	public bool IsPartOfMenu { get; set; } = true;


	//EF Relations	
	public Module? Module { get; init; }

	public ICollection<Operation>? Operations { get; init; }



	private Functionality() { }

	private Functionality(FunctionalityId id, ModuleId moduleId, string name, string abbreviation, string route, string friendlyId, bool isActive, bool isPartOfMenu) : base(id)
	{
		ModuleId = moduleId;
		Name = name;
		Abbreviation = abbreviation;
		Route = route;
		FriendlyId = friendlyId;
		IsActive = isActive;
		IsPartOfMenu = isPartOfMenu;
	}

	public static Functionality Create(ModuleId moduleId, string name, string abbreviation, string route, string friendlyId, bool isPartOfMenu)
	{
		var function = new Functionality(
			new FunctionalityId(Guid.NewGuid()),
			moduleId,
			name,
			abbreviation,
			route,
			friendlyId,
			true,
			isPartOfMenu
		);

		return function;
	}

	public void Update(string name, string abbreviation, string route, string friendlyId, bool isPartOfMenu)
	{
		Name = name;
		Abbreviation = abbreviation;
		Route = route;
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
