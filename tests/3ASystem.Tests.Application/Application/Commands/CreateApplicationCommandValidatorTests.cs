using _3ASystem.Application.UseCases.Applications.Commands.CreateApplication;
using FluentAssertions;

namespace _3ASystem.Tests.Application.Application.Commands;

public class CreateApplicationCommandValidatorTests
{

	[Fact(DisplayName = "CreateApplicationCommandValidator Should Not Trigger Any Validation Issue When A Fully Filed Command Is Used")]
	public async Task CreateApplicationCommandValidator_Should_NotThrowValidationError_WhenFullyFiledCommandIsUsed()
	{
		// Arrange
		var command = new CreateApplicationCommand
		{
			Name = "Ultimate Productivity Suite: Task Management, Collaboration, Time Tracking, and Workflow for Teams",
			Abbreviation = "UPSuite",
			Description = @"Ultimate Productivity Suite: Task Management, Collaboration, Time Tracking, and Workflow Automation for Teams

Unlock the full potential of your team with the Ultimate Productivity Suite, the all-in-one solution designed to streamline your workflow 
and boost efficiency. This comprehensive platform combines powerful task management, seamless collaboration, precise time tracking, 
and intelligent workflow automation to help your team achieve more, faster.",

			IconUrl = "https://test.com/icon.png",
			FriendlyId = "UPSuite_FriendlyId"
		};

		var validator = new CreateApplicationCommandValidator();

		//Act
		var result = await validator.ValidateAsync(command);

		//Assert
		result.IsValid.Should().BeTrue();
	}

	[Fact(DisplayName = "CreateApplicationCommandValidator Should Throw A Validation Error When Name Is Greater Than 100 Chars")]
	public async Task CreateApplicationCommandValidator_Should_ThrowValidationError_WhenNameGreaterThan100Chars()
	{
		// Arrange
		var command = new CreateApplicationCommand
		{
			Name = "Ultimate Productivity Suite: Task Management, Collaboration, Time Tracking, and Workflow Automation for Teams",
			Abbreviation = "UPSuite",
			Description = @"Ultimate Productivity Suite: Task Management, Collaboration, Time Tracking, and Workflow Automation for Teams

Unlock the full potential of your team with the Ultimate Productivity Suite, the all-in-one solution designed to streamline your workflow 
and boost efficiency. This comprehensive platform combines powerful task management, seamless collaboration, precise time tracking, 
and intelligent workflow automation to help your team achieve more, faster.",

			IconUrl = "https://test.com/icon.png",
			FriendlyId = "UPSuite_FriendlyId"
		};

		var validator = new CreateApplicationCommandValidator();

		//Act
		var result = await validator.ValidateAsync(command);

		//Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle(e => e.PropertyName == "Name");
		result.Errors.Should().Contain(e => e.ErrorCode == "MaximumLengthValidator");

	}


	[Fact(DisplayName = "CreateApplicationCommandValidator Should Throw A Validation Error When Abbreviation Is Greater Than 25 Chars")]
	public async Task CreateApplicationCommandValidator_Should_ThrowValidationError_WhenAbbreviationGreaterThan25Chars()
	{
		// Arrange
		var command = new CreateApplicationCommand
		{
			Name = "Ultimate Suite: Task Management, Collaboration, Time Tracking, Workflow Automation for Teams",

			//"UP Suite: Task, Collab, Time"
			Abbreviation = "UP Suite: Task, Collab, Time, WF",
			Description = @"Ultimate (Productivity) Suite: Task Management, Collaboration, Time Tracking, and Workflow Automation for Teams

Unlock the full potential of your team with the Ultimate Productivity Suite, the all-in-one solution designed to streamline your workflow 
and boost efficiency. This comprehensive platform combines powerful task management, seamless collaboration, precise time tracking, 
and intelligent workflow automation to help your team achieve more, faster.",

			IconUrl = "https://test.com/icon.png",
			FriendlyId = "UPSuite_FriendlyId"
		};

		var validator = new CreateApplicationCommandValidator();

		//Act
		var result = await validator.ValidateAsync(command);

		//Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle(e => e.PropertyName == "Abbreviation");
		result.Errors.Should().Contain(e => e.ErrorCode == "MaximumLengthValidator");

	}


	[Fact(DisplayName = "CreateApplicationCommandValidator Should Throw A Validation Error When Name Is Not Provided")]
	public async Task CreateApplicationCommandValidator_Should_ThrowValidationError_WhenNameIsNotProvided()
	{
		// Arrange
		var command = new CreateApplicationCommand
		{
			Name = "",

			Abbreviation = "UPSuite",
			Description = @"Ultimate (Productivity) Suite: Task Management, Collaboration, Time Tracking, and Workflow Automation for Teams

Unlock the full potential of your team with the Ultimate Productivity Suite, the all-in-one solution designed to streamline your workflow 
and boost efficiency. This comprehensive platform combines powerful task management, seamless collaboration, precise time tracking, 
and intelligent workflow automation to help your team achieve more, faster.",

			IconUrl = "https://test.com/icon.png",
			FriendlyId = "UPSuite_FriendlyId"
		};

		var validator = new CreateApplicationCommandValidator();

		//Act
		var result = await validator.ValidateAsync(command);

		//Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle(e => e.PropertyName == "Name");
		result.Errors.Should().Contain(e => e.ErrorCode == "NotEmptyValidator");

	}


	[Fact(DisplayName = "CreateApplicationCommandValidator Should Throw A Validation Error When Abbreviation Is Not Provided")]
	public async Task CreateApplicationCommandValidator_Should_ThrowValidationError_WhenAbbreviationIsNotProvided()
	{
		// Arrange
		var command = new CreateApplicationCommand
		{
			Name = "Ultimate Suite: Task Management, Collaboration, Time Tracking, Workflow Automation for Teams",

			Abbreviation = "",
			Description = @"Ultimate (Productivity) Suite: Task Management, Collaboration, Time Tracking, and Workflow Automation for Teams

Unlock the full potential of your team with the Ultimate Productivity Suite, the all-in-one solution designed to streamline your workflow 
and boost efficiency. This comprehensive platform combines powerful task management, seamless collaboration, precise time tracking, 
and intelligent workflow automation to help your team achieve more, faster.",

			IconUrl = "https://test.com/icon.png",
			FriendlyId = "UPSuite_FriendlyId"
		};

		var validator = new CreateApplicationCommandValidator();

		//Act
		var result = await validator.ValidateAsync(command);


		//Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle(e => e.PropertyName == "Abbreviation");
		result.Errors.Should().Contain(e => e.ErrorCode == "NotEmptyValidator");

	}


	[Fact(DisplayName = "CreateApplicationCommandValidator Should Throw A Validation Error When Description Is Not Provided")]
	public async Task CreateApplicationCommandValidator_Should_ThrowValidationError_WhenDescriptionIsNotProvided()
	{
		// Arrange
		var command = new CreateApplicationCommand
		{
			Name = "Ultimate Suite: Task Management, Collaboration, Time Tracking, Workflow Automation for Teams",

			//"UP Suite: Task, Collab, Time"
			Abbreviation = "UPSuite",
			Description = "",

			IconUrl = "https://test.com/icon.png",
			FriendlyId = "UPSuite_FriendlyId"
		};

		var validator = new CreateApplicationCommandValidator();

		//Act
		var result = await validator.ValidateAsync(command);

		//Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle(e => e.PropertyName == "Description");
		result.Errors.Should().Contain(e => e.ErrorCode == "NotEmptyValidator");

	}


	[Fact(DisplayName = "CreateApplicationCommandValidator Should Throw A Validation Error When FriendlyId Is Not Provided")]
	public async Task CreateApplicationCommandValidator_Should_ThrowValidationError_WhenFriendlyIdIsNotProvided()
	{
		// Arrange
		var command = new CreateApplicationCommand
		{
			Name = "Ultimate Suite: Task Management, Collaboration, Time Tracking, Workflow Automation for Teams",

			Abbreviation = "UPSuite",
			Description = @"Ultimate (Productivity) Suite: Task Management, Collaboration, Time Tracking, and Workflow Automation for Teams

Unlock the full potential of your team with the Ultimate Productivity Suite, the all-in-one solution designed to streamline your workflow 
and boost efficiency. This comprehensive platform combines powerful task management, seamless collaboration, precise time tracking, 
and intelligent workflow automation to help your team achieve more, faster.",

			IconUrl = "https://test.com/icon.png"
			
		};

		var validator = new CreateApplicationCommandValidator();

		//Act
		var result = await validator.ValidateAsync(command);

		//Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle(e => e.PropertyName == "FriendlyId");
		result.Errors.Should().Contain(e => e.ErrorCode == "NotEmptyValidator");

	}

	[Fact(DisplayName = "CreateApplicationCommandValidator Should Throw A Validation Error When FriendlyId Is Not Provided")]
	public async Task CreateApplicationCommandValidator_Should_ThrowValidationError_WhenFriendlyIdIsGreaterThan25Chars()
	{
		// Arrange
		var command = new CreateApplicationCommand
		{
			Name = "Ultimate Suite: Task Management, Collaboration, Time Tracking, Workflow Automation for Teams",

			Abbreviation = "UPSuite",
			Description = @"Ultimate (Productivity) Suite: Task Management, Collaboration, Time Tracking, and Workflow Automation for Teams

Unlock the full potential of your team with the Ultimate Productivity Suite, the all-in-one solution designed to streamline your workflow 
and boost efficiency. This comprehensive platform combines powerful task management, seamless collaboration, precise time tracking, 
and intelligent workflow automation to help your team achieve more, faster.",

			IconUrl = "https://test.com/icon.png",
			FriendlyId = "UPSuite_FriendlyId-UPSuite_FriendlyId"

		};

		var validator = new CreateApplicationCommandValidator();

		//Act
		var result = await validator.ValidateAsync(command);


		//Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle(e => e.PropertyName == "FriendlyId");
		result.Errors.Should().Contain(e => e.ErrorCode == "MaximumLengthValidator");

	}



}
