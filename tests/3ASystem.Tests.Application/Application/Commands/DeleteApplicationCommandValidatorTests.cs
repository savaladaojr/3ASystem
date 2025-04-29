using _3ASystem.Application.UseCases.Applications.Commands.DeleteApplication;
using FluentAssertions;

namespace _3ASystem.Tests.Application.Application.Commands;

public class DeleteApplicationCommandValidatorTests
{

	[Fact(DisplayName = "DeleteApplicationCommandValidator Should Not Trigger Any Validation Issue When A Fully Filled Command Is Used")]
	public async Task DeleteApplicationCommandValidator_Should_NotThrowValidationError_WhenFullyFiledCommandIsUsed()
	{
		// Arrange
		var command = new DeleteApplicationCommand(Guid.NewGuid());

		var validator = new DeleteApplicationCommandValidator();

		// Act
		var result = await validator.ValidateAsync(command);

		//Assert
		result.IsValid.Should().BeTrue();
	}

	[Fact(DisplayName = "DeleteApplicationCommandValidator Should Throw A Validation Error When Id Is Not Provided")]
	public async Task DeleteApplicationCommandValidator_Should_ThrowValidationError_WhenIdIsNotProvided()
	{
		// Arrange
		var command = new DeleteApplicationCommand(Guid.Empty);

		var validator = new DeleteApplicationCommandValidator();

		// Act
		var result = await validator.ValidateAsync(command);

		//Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle(e => e.PropertyName == "Id");
		result.Errors.Should().Contain(e => e.ErrorCode == "NotEmptyValidator");
	}

}
