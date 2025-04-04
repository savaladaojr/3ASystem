﻿using _3ASystem.Domain.Entities.Functionalities;
using _3ASystem.Domain.Entities.Roles;
using _3ASystem.Domain.Shared;
using System.Xml.Linq;

namespace _3ASystem.Domain.Entities.Applications;

public sealed class App : Entity<AppId>
{
	public string Name { get; private set; } = string.Empty;
	public string Abbreviation { get; private set; } = string.Empty;
	public string Description { get; private set; } = string.Empty;
	public string IconUrl { get; private set; } = string.Empty;

	public Guid Hash { get; private set; }

	public bool IsActive { get; private set; } = true;


	//EF Relations
	public ICollection<Functionality>? Functionalities { get; init; }

	public ICollection<Role>? Roles { get; init; }
	

	private App()
	{
	}

	private App(AppId id, string name, string abbreviation, string description, string iconUrl, Guid hash, bool isActive) : base(id)
	{
		Name = name;
		Abbreviation = abbreviation;
		Description = description;
		IconUrl = iconUrl;
		Hash = hash;
		IsActive = isActive;

		Functionalities = new List<Functionality>();
		Roles = new List<Role>();
	}

	public static App Create(string name, string abbreviation, string description, string iconUrl)
	{
		var app = new App
		(
			new AppId(Guid.NewGuid()),
			name,
			abbreviation,
			description,
			iconUrl,
			Guid.NewGuid(),
			true
		);

		return app;
	}

	public void Update(string name, string abbreviation, string description, string iconUrl)
	{
		Name = name;
		Abbreviation = abbreviation;
		Description = description;
		IconUrl = iconUrl;
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
