using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Applications.Commands.CreateApplication;
using _3ASystem.Domain.Data.Repositories;
using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Shared;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ASystem.Tests.Application.Application.Commands;

public class CreateApplicationCommandValidatorTests
{
	Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
	Mock<IAppRepository> _appRepositoryMock = new Mock<IAppRepository>();

	[Fact(DisplayName = "CreateApplicationCommandValidator Should Not Trigger Any Validation Issue When A Command Validation Happen")]
	public async Task CreateApplicationCommandValidator_Should_NotThrowValidationError_WhenValidationHappen()
	{
		// Arrange
		var appExistent = App.Create(
			"Test Application Existent",
			"TA",
			"Test Application Existent Description",
			"https://test.com/icon.png"
		);

		_appRepositoryMock.Setup(
			repo => repo.GetByAbbreviationAsync(It.IsAny<string>())
		).ReturnsAsync(appExistent);
		
		var command = new CreateApplicationCommand
		{
			Name = "Test Application",
			Abbreviation = "TA",
			Description = "Test Application Description",
			IconUrl = "https://test.com/icon.png"
		};

		var validator = new CreateApplicationCommandValidator();
		var result = await validator.ValidateAsync(command);

		result.IsValid.Should().BeTrue();
	}

	[Fact(DisplayName = "CreateApplicationCommandValidator Should Throw A Validation Error When Name Is Greater Than 100 Chars")]
	public async Task CreateApplicationCommandValidator_Should_ThrowValidationError_WhenNameGreaterThan100Chars()
	{
		// Arrange
		var appExistent = App.Create(
			"Test Application Existent",
			"TA",
			"Test Application Existent Description",
			"https://test.com/icon.png"
		);

		_appRepositoryMock.Setup(
			repo => repo.GetByAbbreviationAsync(It.IsAny<string>())
		).ReturnsAsync(appExistent);

		var command = new CreateApplicationCommand
		{
			Name = "Ultimate Productivity Suite: Task Management, Collaboration, Time Tracking, and Workflow Automation for Teams",
			Abbreviation = "UPS",
			Description = @"Ultimate Productivity Suite: Task Management, Collaboration, Time Tracking, and Workflow Automation for Teams

Unlock the full potential of your team with the Ultimate Productivity Suite, the all-in-one solution designed to streamline your workflow 
and boost efficiency. This comprehensive platform combines powerful task management, seamless collaboration, precise time tracking, 
and intelligent workflow automation to help your team achieve more, faster.",

			IconUrl = "https://test.com/icon.png"
		};

		var validator = new CreateApplicationCommandValidator();
		var result = await validator.ValidateAsync(command);

		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle(e => e.PropertyName == "Name");
		result.Errors.Should().Contain(e => e.ErrorCode == "MaximumLengthValidator");
		//result.Errors.Should().ContainSingle(e => e.ErrorMessage == "'Name' must not be empty.");

	}


	[Fact(DisplayName = "CreateApplicationCommandValidator Should Throw A Validation Error When Abbreviation Is Greater Than 25 Chars")]
	public async Task CreateApplicationCommandValidator_Should_ThrowValidationError_WhenAbbreviationGreaterThan25Chars()
	{
		// Arrange
		var appExistent = App.Create(
			"Test Application Existent",
			"TA",
			"Test Application Existent Description",
			"https://test.com/icon.png"
		);

		_appRepositoryMock.Setup(
			repo => repo.GetByAbbreviationAsync(It.IsAny<string>())
		).ReturnsAsync(appExistent);

		var command = new CreateApplicationCommand
		{
			Name = "Ultimate Suite: Task Management, Collaboration, Time Tracking, Workflow Automation for Teams",

			//"UP Suite: Task, Collab, Time"
			Abbreviation = "UP Suite: Task, Collab, Time, WF",
			Description = @"Ultimate (Productivity) Suite: Task Management, Collaboration, Time Tracking, and Workflow Automation for Teams

Unlock the full potential of your team with the Ultimate Productivity Suite, the all-in-one solution designed to streamline your workflow 
and boost efficiency. This comprehensive platform combines powerful task management, seamless collaboration, precise time tracking, 
and intelligent workflow automation to help your team achieve more, faster.",

			IconUrl = "https://test.com/icon.png"
		};

		var validator = new CreateApplicationCommandValidator();
		var result = await validator.ValidateAsync(command);

		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle(e => e.PropertyName == "Abbreviation");
		result.Errors.Should().Contain(e => e.ErrorCode == "MaximumLengthValidator");

	}

}
