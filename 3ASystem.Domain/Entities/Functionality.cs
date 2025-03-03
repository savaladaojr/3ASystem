
using _3ASystem.Domain.Abstractions;
using _3ASystem.Domain.Entities.Identifiers;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;

namespace _3ASystem.Domain.Entities
{
	public sealed class Functionality : Entity<FunctionalityId>
	{
		public AppId ApplicationId { get; private set; } = default!;
		public string Name { get; private set; } = default!;
		public string Abbreviation { get; private set; } = default!;
		public string Route { get; private set; } = default!;

		public bool IsActive { get; set; } = true;


		//EF Relations	
		public App? Application { get; init; }

		public ICollection<Operation> Operations { get; init; } = [];



		private Functionality() { }

		private Functionality(FunctionalityId id, AppId appId, string name, string abbreviation, string route, bool isActive) : base(id)
		{
			ApplicationId = appId;
			Name = name;
			Abbreviation = abbreviation;
			Route = route;
			IsActive = isActive;
		}

		public static Functionality Create(AppId appId, string name, string abbreviation, string route)
		{
			var function = new Functionality(
				new FunctionalityId(Guid.NewGuid()),
				appId,
				name,
				abbreviation,
				route,
				true
			);

			return function;
		}

		public void Update(string name, string abbreviation, string route)
		{
			Name = name;
			Abbreviation = abbreviation;
			Route = route;
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
