using _3ASystem.Domain.Abstractions;
using _3ASystem.Domain.Entities.Identifiers;
using System.Xml.Linq;

namespace _3ASystem.Domain.Entities
{
	public sealed class App : Entity<AppId>
	{
		public string Name { get; private set; } = string.Empty;
		public string Abbreviation { get; private set; } = string.Empty;
		public string Description { get; private set; } = string.Empty;
		public string IconUrl { get; private set; } = string.Empty;

		public Guid Hash { get; private set; }

		public bool IsActive { get; private set; } = true;


		//EF Relations
		public ICollection<Functionality> Functionalities { get; init; } = [];
		public ICollection<Role> Roles { get; init; } = [];
		

		private App()
		{
		}

		private App(AppId id, string name, string abbreviation, Guid hash, bool isActive) : base(id)
		{
			Name = name;
			Abbreviation = abbreviation;
			Hash = hash;

			IsActive = isActive;
		}

		public static App Create(string name, string abbreviation)
		{
			var app = new App
			(
				new AppId(Guid.NewGuid()),
				name,
				abbreviation,
				Guid.NewGuid(),
				true
			);

			return app;
		}

		public void Update(string name, string abbreviation)
		{
			Name = name;
			Abbreviation = abbreviation;
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
