using _3ASystem.Application.UseCases.Applications.Commands.UpdateApplication;
using FluentAssertions;

namespace _3ASystem.Tests.Application.Application.Commands;

public class UpdateApplicationCommandValidatorTests
{
	[Fact(DisplayName = "UpdateApplicationCommandValidator Should Not Trigger Any Validation Issue When A Fully Filled Command Is Used")]
	public async Task UpdateApplicationCommandValidator_Should_NotThrowValidationError_WhenFullyFiledCommandIsUsed()
	{
		//Arrange
		var command = new UpdateApplicationCommand
		{
			Id = Guid.NewGuid(),
			Name = "Ultimate Productivity Suite: Task Management, Collaboration, Time Tracking, and Workflow for Teams",
			Abbreviation = "UPSuite",
			Description = @"Ultimate Productivity Suite: Task Management, Collaboration, Time Tracking, and Workflow Automation for Teams

Unlock the full potential of your team with the Ultimate Productivity Suite, the all-in-one solution designed to streamline your workflow 
and boost efficiency. This comprehensive platform combines powerful task management, seamless collaboration, precise time tracking, 
and intelligent workflow automation to help your team achieve more, faster.",

			IconUrl = "https://images.icon-icons.com/4231/PNG/32/building_city_icon_263721.png",
			FriendlyId = "UPSuite_FriendlyId"
		};

		var validator = new UpdateApplicationCommandValidator();

		//Act
		var result = await validator.ValidateAsync(command);

		//Assert
		result.IsValid.Should().BeTrue();
	}

	[Fact(DisplayName = "UpdateApplicationCommandValidator Should Throw A Validation Error When Id Is not Provided")]
	public async Task UpdateApplicationCommandValidator_Should_ThrowValidationError_WhenIdIsNotProvided()
	{
		//Arrange
		var command = new UpdateApplicationCommand
		{
			Id = Guid.Empty,
			Name = "Ultimate Suite: Task Management, Collaboration, Time Tracking, Workflow Automation for Teams",
			Abbreviation = "UPSuite",
			Description = @"Ultimate Productivity Suite: Task Management, Collaboration, Time Tracking, and Workflow Automation for Teams

Unlock the full potential of your team with the Ultimate Productivity Suite, the all-in-one solution designed to streamline your workflow 
and boost efficiency. This comprehensive platform combines powerful task management, seamless collaboration, precise time tracking, 
and intelligent workflow automation to help your team achieve more, faster.",

			IconUrl = "https://test.com/icon.png",
			FriendlyId = "UPSuite_FriendlyId"
		};

		var validator = new UpdateApplicationCommandValidator();

		//Act
		var result = await validator.ValidateAsync(command);

		//Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle(e => e.PropertyName == "Id");
		result.Errors.Should().Contain(e => e.ErrorCode == "NotEmptyValidator");

	}



	[Fact(DisplayName = "UpdateApplicationCommandValidator Should Throw A Validation Error When Name Is Greater Than 100 Chars")]
	public async Task UpdateApplicationCommandValidator_Should_ThrowValidationError_WhenNameGreaterThan100Chars()
	{
		//Arrange
		var command = new UpdateApplicationCommand
		{
			Id = Guid.NewGuid(),
			Name = "Ultimate Productivity Suite: Task Management, Collaboration, Time Tracking, and Workflow Automation for Teams",
			Abbreviation = "UPSuite",
			Description = @"Ultimate Productivity Suite: Task Management, Collaboration, Time Tracking, and Workflow Automation for Teams

Unlock the full potential of your team with the Ultimate Productivity Suite, the all-in-one solution designed to streamline your workflow 
and boost efficiency. This comprehensive platform combines powerful task management, seamless collaboration, precise time tracking, 
and intelligent workflow automation to help your team achieve more, faster.",

			IconUrl = "https://test.com/icon.png",
			FriendlyId = "UPSuite_FriendlyId"
		};

		var validator = new UpdateApplicationCommandValidator();

		//Act
		var result = await validator.ValidateAsync(command);

		//Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle(e => e.PropertyName == "Name");
		result.Errors.Should().Contain(e => e.ErrorCode == "MaximumLengthValidator");

	}


	[Fact(DisplayName = "UpdateApplicationCommandValidator Should Throw A Validation Error When Abbreviation Is Greater Than 25 Chars")]
	public async Task UpdateApplicationCommandValidator_Should_ThrowValidationError_WhenAbbreviationGreaterThan25Chars()
	{
		// Arrange
		var command = new UpdateApplicationCommand
		{
			Id = Guid.NewGuid(),
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

		var validator = new UpdateApplicationCommandValidator();

		//Act
		var result = await validator.ValidateAsync(command);

		//Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle(e => e.PropertyName == "Abbreviation");
		result.Errors.Should().Contain(e => e.ErrorCode == "MaximumLengthValidator");

	}


	[Fact(DisplayName = "UpdateApplicationCommandValidator Should Throw A Validation Error When Name Is Not Provided")]
	public async Task UpdateApplicationCommandValidator_Should_ThrowValidationError_WhenNameIsNotProvided()
	{
		// Arrange
		var command = new UpdateApplicationCommand
		{
			Id = Guid.NewGuid(),
			Name = "",

			Abbreviation = "UPSuite",
			Description = @"Ultimate (Productivity) Suite: Task Management, Collaboration, Time Tracking, and Workflow Automation for Teams

Unlock the full potential of your team with the Ultimate Productivity Suite, the all-in-one solution designed to streamline your workflow 
and boost efficiency. This comprehensive platform combines powerful task management, seamless collaboration, precise time tracking, 
and intelligent workflow automation to help your team achieve more, faster.",

			IconUrl = "https://test.com/icon.png",
			FriendlyId = "UPSuite_FriendlyId"
		};

		var validator = new UpdateApplicationCommandValidator();

		//Act
		var result = await validator.ValidateAsync(command);

		//Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle(e => e.PropertyName == "Name");
		result.Errors.Should().Contain(e => e.ErrorCode == "NotEmptyValidator");

	}


	[Fact(DisplayName = "UpdateApplicationCommandValidator Should Throw A Validation Error When Abbreviation Is Not Provided")]
	public async Task UpdateApplicationCommandValidator_Should_ThrowValidationError_WhenAbbreviationIsNotProvided()
	{
		// Arrange
		var command = new UpdateApplicationCommand
		{
			Id = Guid.NewGuid(),
			Name = "Ultimate Suite: Task Management, Collaboration, Time Tracking, Workflow Automation for Teams",

			Abbreviation = "",
			Description = @"Ultimate (Productivity) Suite: Task Management, Collaboration, Time Tracking, and Workflow Automation for Teams

Unlock the full potential of your team with the Ultimate Productivity Suite, the all-in-one solution designed to streamline your workflow 
and boost efficiency. This comprehensive platform combines powerful task management, seamless collaboration, precise time tracking, 
and intelligent workflow automation to help your team achieve more, faster.",

			IconUrl = "https://test.com/icon.png",
			FriendlyId = "UPSuite_FriendlyId"
		};

		var validator = new UpdateApplicationCommandValidator();

		//Act
		var result = await validator.ValidateAsync(command);


		//Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle(e => e.PropertyName == "Abbreviation");
		result.Errors.Should().Contain(e => e.ErrorCode == "NotEmptyValidator");

	}


	[Fact(DisplayName = "UpdateApplicationCommandValidator Should Throw A Validation Error When Description Is Not Provided")]
	public async Task UpdateApplicationCommandValidator_Should_ThrowValidationError_WhenDescriptionIsNotProvided()
	{
		// Arrange
		var command = new UpdateApplicationCommand
		{
			Id = Guid.NewGuid(),
			Name = "Ultimate Suite: Task Management, Collaboration, Time Tracking, Workflow Automation for Teams",

			//"UP Suite: Task, Collab, Time"
			Abbreviation = "UPSuite",
			Description = "",

			IconUrl = "https://test.com/icon.png",
			FriendlyId = "UPSuite_FriendlyId"
		};

		var validator = new UpdateApplicationCommandValidator();

		//Act
		var result = await validator.ValidateAsync(command);

		//Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle(e => e.PropertyName == "Description");
		result.Errors.Should().Contain(e => e.ErrorCode == "NotEmptyValidator");

	}


	[Fact(DisplayName = "UpdateApplicationCommandValidator Should Throw A Validation Error When FriendlyId Is Not Provided")]
	public async Task UpdateApplicationCommandValidator_Should_ThrowValidationError_WhenFriendlyIdIsNotProvided()
	{
		// Arrange
		var command = new UpdateApplicationCommand
		{
			Id = Guid.NewGuid(),
			Name = "Ultimate Suite: Task Management, Collaboration, Time Tracking, Workflow Automation for Teams",

			Abbreviation = "UPSuite",
			Description = @"Ultimate (Productivity) Suite: Task Management, Collaboration, Time Tracking, and Workflow Automation for Teams

Unlock the full potential of your team with the Ultimate Productivity Suite, the all-in-one solution designed to streamline your workflow 
and boost efficiency. This comprehensive platform combines powerful task management, seamless collaboration, precise time tracking, 
and intelligent workflow automation to help your team achieve more, faster.",

			IconUrl = "https://test.com/icon.png"

		};

		var validator = new UpdateApplicationCommandValidator();

		//Act
		var result = await validator.ValidateAsync(command);

		//Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle(e => e.PropertyName == "FriendlyId");
		result.Errors.Should().Contain(e => e.ErrorCode == "NotEmptyValidator");

	}

	[Fact(DisplayName = "UpdateApplicationCommandValidator Should Throw A Validation Error When FriendlyId Is Not Provided")]
	public async Task UpdateApplicationCommandValidator_Should_ThrowValidationError_WhenFriendlyIdIsGreaterThan25Chars()
	{
		// Arrange
		var command = new UpdateApplicationCommand
		{
			Id = Guid.NewGuid(),
			Name = "Ultimate Suite: Task Management, Collaboration, Time Tracking, Workflow Automation for Teams",

			Abbreviation = "UPSuite",
			Description = @"Ultimate (Productivity) Suite: Task Management, Collaboration, Time Tracking, and Workflow Automation for Teams

Unlock the full potential of your team with the Ultimate Productivity Suite, the all-in-one solution designed to streamline your workflow 
and boost efficiency. This comprehensive platform combines powerful task management, seamless collaboration, precise time tracking, 
and intelligent workflow automation to help your team achieve more, faster.",

			IconUrl = "https://test.com/icon.png",
			FriendlyId = "UPSuite_FriendlyId-UPSuite_FriendlyId"

		};

		var validator = new UpdateApplicationCommandValidator();

		//Act
		var result = await validator.ValidateAsync(command);

		//Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle(e => e.PropertyName == "FriendlyId");
		result.Errors.Should().Contain(e => e.ErrorCode == "MaximumLengthValidator");

	}


}
