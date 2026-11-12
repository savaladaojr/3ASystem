using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.UseCases.Operations.Commands.CreateOperation;
using _3ASystem.Application.UseCases.Operations.Responses;
using _3ASystem.Domain.Entities.Functionalities;
using _3ASystem.Domain.Entities.Operations;
using _3ASystem.Domain.Shared;
using FluentAssertions;
using NSubstitute;

namespace _3ASystem.Tests.Application.Application.Commands;

public class CreateOperationCommandHandlerTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IOperationRepository _operationRepository = Substitute.For<IOperationRepository>();
    private readonly IFunctionalityRepository _functionalityRepository = Substitute.For<IFunctionalityRepository>();

    [Fact(DisplayName = "CreateOperationCommandHandler Should Return Fail Result When Code Is Not Unique")]
    public async Task CreateOperationCommandHandler_Should_ReturnFailResult_WhenCodeIsNotUnique()
    {
        // Arrange
        var handler = new CreateOperationCommandHandler(_operationRepository, _functionalityRepository, _unitOfWork);

        var functionalityId = new FunctionalityId(Guid.NewGuid());
        var command = new CreateOperationCommand
        {
            FunctionalityId = functionalityId.Value,
            Name = "Test Operation",
            Code = "OP001",
            FriendlyId = "op-test"
        };

        _operationRepository.CodeExistsAsync(command.Code).Returns(true);

        // Act
        Result<OperationDetailedResponse> result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(OperationErrors.CodeNotUnique);
    }

    [Fact(DisplayName = "CreateOperationCommandHandler Should Return Fail Result When FriendlyId Is Not Unique")]
    public async Task CreateOperationCommandHandler_Should_ReturnFailResult_WhenFriendlyIdIsNotUnique()
    {
        // Arrange
        var handler = new CreateOperationCommandHandler(_operationRepository, _functionalityRepository, _unitOfWork);

        var functionalityId = new FunctionalityId(Guid.NewGuid());
        var command = new CreateOperationCommand
        {
            FunctionalityId = functionalityId.Value,
            Name = "Test Operation",
            Code = "OP001",
            FriendlyId = "op-test"
        };

        _operationRepository.CodeExistsAsync(command.Code).Returns(false);
        _operationRepository.FriendlyIdExistsAsync(command.FriendlyId).Returns(true);

        // Act
        Result<OperationDetailedResponse> result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(OperationErrors.FriendlyIdNotUnique);
    }

    [Fact(DisplayName = "CreateOperationCommandHandler Should Return Fail Result When Functionality Does Not Exist")]
    public async Task CreateOperationCommandHandler_Should_ReturnFailResult_WhenFunctionalityDoesNotExist()
    {
        // Arrange
        var handler = new CreateOperationCommandHandler(_operationRepository, _functionalityRepository, _unitOfWork);

        var functionalityId = new FunctionalityId(Guid.NewGuid());
        var command = new CreateOperationCommand
        {
            FunctionalityId = functionalityId.Value,
            Name = "Test Operation",
            Code = "OP001",
            FriendlyId = "op-test"
        };

        _operationRepository.CodeExistsAsync(command.Code).Returns(false);
        _operationRepository.FriendlyIdExistsAsync(command.FriendlyId).Returns(false);
        _functionalityRepository.GetByIdAsync(functionalityId).Returns((Functionality?)null);

        // Act
        Result<OperationDetailedResponse> result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Contain("Functionalities.NotFound");
    }
}
