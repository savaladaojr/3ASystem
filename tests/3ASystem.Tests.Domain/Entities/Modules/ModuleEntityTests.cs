using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Entities.Modules;
using FluentAssertions;

namespace _3ASystem.Tests.Domain.Entities.Application;

public class ModuleEntityTests
{
	[Fact(DisplayName = "Module Entity Should Create New Module When Static Create Method is Called With Required Parameters.")]
	public void ModuleEntity_Should_CreateNewModuleObject_WhenStaticMethodIsCalledWithParameters()
	{
		//Arrange
		var AppId = new AppId(Guid.NewGuid());
		var sName = "Module 1";
		var sAbbreviation = "MDL1";
		var sDescription = "Application's Module 1 Description";
		var sIcon = "icon.png";
		var sFriendlyId = "APL1_MDL1_FRIENDLYID";
		var isPartOfMenu = true;

		//Act
		var module = Module.Create(AppId, sName, sAbbreviation, sDescription, sIcon, sFriendlyId, isPartOfMenu);

		//Assert
		module.Should().NotBeNull();
		module.Id.Value.Should().NotBe(Guid.Empty);

	}

	[Fact(DisplayName = "Module Entity Should Update An Existent Module Properties When Object's Update Method Is Called With Parameters")]
	public void ModuleEntity_Should_UpdateAnExistentModuleObject_WhenObjectUpdateMethodIsCalledWithParameters()
	{
		//Arrange
		var AppId = new AppId(Guid.NewGuid());
		var sName = "Module 1";
		var sAbbreviation = "MDL1";
		var sDescription = "Application's Module 1 Description";
		var sIcon = "icon.png";
		var sFriendlyId = "APL1_MDL1_FRIENDLYID";
		var isPartOfMenu = true;

		var module = Module.Create(AppId, sName, sAbbreviation, sDescription, sIcon, sFriendlyId, isPartOfMenu);

		var sDescriptionUpdated = "Module 1 Description Update";

		//Act
		module.Update(module.Name, module.Abbreviation, sDescriptionUpdated, module.IconUrl, module.FriendlyId, module.IsPartOfMenu);

		//Assert
		Assert.Equal(sDescriptionUpdated, module.Description);
	}

	[Fact(DisplayName = "Module Entity Should Enable An Existent Module When Object's Enable Method Is Called.")]
	public void ModuleEntity_Should_EnableAnExistentModuleObject_WhenObjectEnabledMethodIsCalled()
	{
		//Arrange
		var AppId = new AppId(Guid.NewGuid());
		var sName = "Module 1";
		var sAbbreviation = "MDL1";
		var sDescription = "Application's Module 1 Description";
		var sIcon = "icon.png";
		var sFriendlyId = "APL1_MDL1_FRIENDLYID";
		var isPartOfMenu = true;

		var module = Module.Create(AppId, sName, sAbbreviation, sDescription, sIcon, sFriendlyId, isPartOfMenu);

		//Act
		module.Enable();

		//Assert
		Assert.True(module.IsActive);
	}

	[Fact(DisplayName = "App Entity Should Disable An Existent Application When Object's Disable Method Is Called.")]
	public void AppEntity_Should_DisableAnExistentApplicationObject_WhenObjectDisableMethodIsCalled()
	{
		//Arrange
		var AppId = new AppId(Guid.NewGuid());
		var sName = "Module 1";
		var sAbbreviation = "MDL1";
		var sDescription = "Application's Module 1 Description";
		var sIcon = "icon.png";
		var sFriendlyId = "APL1_MDL1_FRIENDLYID";
		var isPartOfMenu = true;

		var module = Module.Create(AppId, sName, sAbbreviation, sDescription, sIcon, sFriendlyId, isPartOfMenu);

		//Act
		module.Disable();

		//Assert
		Assert.False(module.IsActive);
	}
}
