using _3ASystem.Application.Applications.Commands.CreateApplication;
using _3ASystem.Application.Applications.Commands.DeleteApplication;
using _3ASystem.Application.Applications.Commands.EnableDisableApplication;
using FluentAssertions;

namespace _3ASystem.Tests.Application.Application.Commands;

public class EnableDisableApplicationCommandValidatorTests
{

	[Fact(DisplayName = "EnableDisableApplicationCommandValidator Should Not Throw Any Validation Issue When A Fully Filled Command Is Used")]
	public async Task EnableDisableApplicationCommandValidator_Should_NotThrowValidationError_WhenFullyFiledCommandIsUsed()
	{
		// Arrange
		var command = new EnableDisableApplicationCommand() { Id = Guid.NewGuid() };

		var validator = new EnableDisableApplicationCommandValidator();

		// Act
		var result = await validator.ValidateAsync(command);

		//Assert
		result.IsValid.Should().BeTrue();
	}

	[Fact(DisplayName = "EnableDisableApplicationCommandValidator Should Throw A Validation Error When Id Is Not Provided")]
	public async Task DeleteApplicationCommandValidator_Should_ThrowValidationError_WhenIdIsNotProvided()
	{
		// Arrange
		var command = new EnableDisableApplicationCommand() { Id = Guid.Empty };

		var validator = new EnableDisableApplicationCommandValidator();

		// Act
		var result = await validator.ValidateAsync(command);

		//Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle(e => e.PropertyName == "Id");
		result.Errors.Should().Contain(e => e.ErrorCode == "NotEmptyValidator");
	}

}
