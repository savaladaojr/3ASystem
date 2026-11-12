using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.UseCases.Operations.Commands.UpdateOperation;
using _3ASystem.Application.UseCases.Operations.Responses;
using _3ASystem.Domain.Entities.Functionalities;
using _3ASystem.Domain.Entities.Operations;
using _3ASystem.Domain.Shared;
using FluentAssertions;
using NSubstitute;

namespace _3ASystem.Tests.Application.Application.Commands;

public class UpdateOperationCommandHandlerTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IOperationRepository _operationRepository = Substitute.For<IOperationRepository>();

    [Fact(DisplayName = "UpdateOperationCommandHandler Should Return Fail Result When Operation Does Not Exist")]
    public async Task UpdateOperationCommandHandler_Should_ReturnFailResult_WhenOperationDoesNotExist()
    {
        // Arrange
        var handler = new UpdateOperationCommandHandler(_operationRepository, _unitOfWork);

        var operationId = new OperationId(Guid.NewGuid());
        var command = new UpdateOperationCommand
        {
            Id = operationId.Value,
            Name = "Updated Operation",
            Code = "OP002",
            FriendlyId = "op-updated"
        };

        _operationRepository.GetByIdAsync(operationId).Returns((Operation?)null);

        // Act
        Result<OperationDetailedResponse> result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Contain("Operations.NotFound");
    }

    [Fact(DisplayName = "UpdateOperationCommandHandler Should Return Fail Result When New Code Already Exists")]
    public async Task UpdateOperationCommandHandler_Should_ReturnFailResult_WhenNewCodeAlreadyExists()
    {
        // Arrange
        var handler = new UpdateOperationCommandHandler(_operationRepository, _unitOfWork);

        var operationId = new OperationId(Guid.NewGuid());
        var functionalityId = new FunctionalityId(Guid.NewGuid());

        var operation = Operation.Create(functionalityId, "Test Operation", "OP001", "op-test");

        var command = new UpdateOperationCommand
        {
            Id = operationId.Value,
            Name = "Updated Operation",
            Code = "OP002",
            FriendlyId = "op-updated"
        };

        _operationRepository.GetByIdAsync(operationId).Returns(operation);
        _operationRepository.CodeExistsAsync(command.Code, operationId).Returns(true);

        // Act
        Result<OperationDetailedResponse> result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(OperationErrors.CodeNotUnique);
    }

    [Fact(DisplayName = "UpdateOperationCommandHandler Should Return Fail Result When New FriendlyId Already Exists")]
    public async Task UpdateOperationCommandHandler_Should_ReturnFailResult_WhenNewFriendlyIdAlreadyExists()
    {
        // Arrange
        var handler = new UpdateOperationCommandHandler(_operationRepository, _unitOfWork);

        var operationId = new OperationId(Guid.NewGuid());
        var functionalityId = new FunctionalityId(Guid.NewGuid());

        var operation = Operation.Create(functionalityId, "Test Operation", "OP001", "op-test");

        var command = new UpdateOperationCommand
        {
            Id = operationId.Value,
            Name = "Updated Operation",
            Code = "OP002",
            FriendlyId = "op-updated"
        };

        _operationRepository.GetByIdAsync(operationId).Returns(operation);
        _operationRepository.CodeExistsAsync(command.Code, operationId).Returns(false);
        _operationRepository.FriendlyIdExistsAsync(command.FriendlyId, operationId).Returns(true);

        // Act
        Result<OperationDetailedResponse> result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(OperationErrors.FriendlyIdNotUnique);
    }

    [Fact(DisplayName = "UpdateOperationCommandHandler Should Successfully Update Operation")]
    public async Task UpdateOperationCommandHandler_Should_SuccessfullyUpdateOperation()
    {
        // Arrange
        var handler = new UpdateOperationCommandHandler(_operationRepository, _unitOfWork);

        var operationId = new OperationId(Guid.NewGuid());
        var functionalityId = new FunctionalityId(Guid.NewGuid());

        var operation = Operation.Create(functionalityId, "Test Operation", "OP001", "op-test");

        var command = new UpdateOperationCommand
        {
            Id = operationId.Value,
            Name = "Updated Operation",
            Code = "OP002",
            FriendlyId = "op-updated"
        };

        _operationRepository.GetByIdAsync(operationId).Returns(operation);
        _operationRepository.CodeExistsAsync(command.Code, operationId).Returns(false);
        _operationRepository.FriendlyIdExistsAsync(command.FriendlyId, operationId).Returns(false);

        // Act
        Result<OperationDetailedResponse> result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Name.Should().Be(command.Name);
        result.Value.Code.Should().Be(command.Code);
        result.Value.FriendlyId.Should().Be(command.FriendlyId);

        _operationRepository.Received(1).Update(operation);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
