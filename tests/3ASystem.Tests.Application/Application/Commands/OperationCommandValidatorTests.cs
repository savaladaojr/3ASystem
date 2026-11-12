using _3ASystem.Application.UseCases.Operations.Commands.CreateOperation;
using _3ASystem.Application.UseCases.Operations.Commands.UpdateOperation;
using FluentAssertions;

namespace _3ASystem.Tests.Application.Application.Commands;

public class OperationCommandValidatorTests
{
    [Fact]
    public void CreateValidator_ShouldAllowFiftyCharacterCodeAndFriendlyId()
    {
        var command = new CreateOperationCommand
        {
            FunctionalityId = Guid.NewGuid(),
            Name = "Operation",
            Code = new string('c', 50),
            FriendlyId = new string('f', 50)
        };

        var result = new CreateOperationCommandValidator().Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void CreateValidator_ShouldRejectFriendlyIdLongerThanDatabaseLimit()
    {
        var command = new CreateOperationCommand
        {
            FunctionalityId = Guid.NewGuid(),
            Name = "Operation",
            Code = "ACC.Read",
            FriendlyId = new string('f', 51)
        };

        var result = new CreateOperationCommandValidator().Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error => error.PropertyName == nameof(command.FriendlyId));
    }

    [Fact]
    public void UpdateValidator_ShouldRejectCodeLongerThanDatabaseLimit()
    {
        var command = new UpdateOperationCommand
        {
            Id = Guid.NewGuid(),
            Name = "Operation",
            Code = new string('c', 51),
            FriendlyId = "accounts-Read"
        };

        var result = new UpdateOperationCommandValidator().Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error => error.PropertyName == nameof(command.Code));
    }
}
