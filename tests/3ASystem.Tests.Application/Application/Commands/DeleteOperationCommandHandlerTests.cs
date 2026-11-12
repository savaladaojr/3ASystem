using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.UseCases.Operations.Commands.DeleteOperation;
using _3ASystem.Domain.Entities.Functionalities;
using _3ASystem.Domain.Entities.Operations;
using _3ASystem.Domain.Shared;
using FluentAssertions;
using NSubstitute;

namespace _3ASystem.Tests.Application.Application.Commands;

public class DeleteOperationCommandHandlerTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IOperationRepository _operationRepository = Substitute.For<IOperationRepository>();

    [Fact(DisplayName = "DeleteOperationCommandHandler Should Return Fail Result When Operation Does Not Exist")]
    public async Task DeleteOperationCommandHandler_Should_ReturnFailResult_WhenOperationDoesNotExist()
    {
        // Arrange
        var handler = new DeleteOperationCommandHandler(_operationRepository, _unitOfWork);

        var operationId = new OperationId(Guid.NewGuid());
        var command = new DeleteOperationCommand { Id = operationId.Value };

        _operationRepository.GetByIdAsync(operationId).Returns((Operation?)null);

        // Act
        Result result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Contain("Operations.NotFound");
    }

    [Fact(DisplayName = "DeleteOperationCommandHandler Should Successfully Delete Operation")]
    public async Task DeleteOperationCommandHandler_Should_SuccessfullyDeleteOperation()
    {
        // Arrange
        var handler = new DeleteOperationCommandHandler(_operationRepository, _unitOfWork);

        var operationId = new OperationId(Guid.NewGuid());
        var functionalityId = new FunctionalityId(Guid.NewGuid());

        var operation = Operation.Create(functionalityId, "Test Operation", "OP001", "op-test");

        var command = new DeleteOperationCommand { Id = operationId.Value };

        _operationRepository.GetByIdAsync(operationId).Returns(operation);

        // Act
        Result result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _operationRepository.Received(1).Delete(operationId);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
