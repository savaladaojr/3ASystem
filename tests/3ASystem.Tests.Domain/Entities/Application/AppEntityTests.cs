using _3ASystem.Domain.Entities.Applications;
using FluentAssertions;
using System.Reflection;

namespace _3ASystem.Tests.Domain.Entities.Application;

public class AppEntityTests
{
	[Fact(DisplayName = "App Entity Should Create New Application When Static Create Method is Called With Required Parameters.")]
	public void AppEntity_Should_CreateNewApplicationObject_WhenStaticMethodIsCalledWithParameters()
	{
		//Arrange
		var sName = "Application 1";
		var sAbbreviation = "APL1";
		var sDescription = "Application 1 Description";
		var sIcon = "icon.png";
		var sFriendlyId = "APL1_FRIENDLYID";

		//Act
		var app = App.Create(sName, sAbbreviation, sDescription, sIcon, sFriendlyId);

		//Assert
		app.Should().NotBeNull();
		app.Id.Value.Should().NotBe(Guid.Empty);

	}

	[Fact(DisplayName = "App Entity Should Update An Existent Application Properties When Object's Update Method Is Called With Parameters")]
	public void AppEntity_Should_UpdateAnExistentApplicationObject_WhenObjectUpdateMethodIsCalledWithParameters()
	{
		//Arrange
		var sName = "Application 1";
		var sAbbreviation = "APL1";
		var sDescription = "Application 1 Description";
		var sIcon = "icon.png";
		var sFriendlyId = "APL1_FRIENDLYID";

		var app = App.Create(sName, sAbbreviation, sDescription, sIcon, sFriendlyId);

		var sDescriptionUpdated = "Application 1 Description Update";

		//Act
		app.Update(app.Name, app.Abbreviation, sDescriptionUpdated, app.IconUrl, sFriendlyId);

		//Assert
		Assert.Equal(sDescriptionUpdated, app.Description);
	}

	[Fact(DisplayName = "App Entity Should Enable An Existent Application When Object's Enable Method Is Called.")]
	public void AppEntity_Should_EnableAnExistentApplicationObject_WhenObjectEnabledMethodIsCalled()
	{
		//Arrange
		var sName = "Application 1";
		var sAbbreviation = "APL1";
		var sDescription = "Application 1 Description";
		var sIcon = "icon.png";
		var sFriendlyId = "APL1_FRIENDLYID";

		var app = App.Create(sName, sAbbreviation, sDescription, sIcon, sFriendlyId);

		//Act
		app.Enable();

		//Assert
		Assert.True(app.IsActive);
	}

	[Fact(DisplayName = "App Entity Should Disable An Existent Application When Object's Disable Method Is Called.")]
	public void AppEntity_Should_DisableAnExistentApplicationObject_WhenObjectDisableMethodIsCalled()
	{
		//Arrange
		var sName = "Application 1";
		var sAbbreviation = "APL1";
		var sDescription = "Application 1 Description";
		var sIcon = "icon.png";
		var sFriendlyId = "APL1_FRIENDLYID";

		var app = App.Create(sName, sAbbreviation, sDescription, sIcon, sFriendlyId);

		//Act
		app.Disable();

		//Assert
		Assert.False(app.IsActive);
	}
}
