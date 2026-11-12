using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.UseCases.Operations.Queries.GetOperationById;
using _3ASystem.Application.UseCases.Operations.Responses;
using _3ASystem.Domain.Entities.Functionalities;
using _3ASystem.Domain.Entities.Operations;
using _3ASystem.Domain.Shared;
using FluentAssertions;
using NSubstitute;

namespace _3ASystem.Tests.Application.Application.Queries;

public class GetOperationByIdQueryHandlerTests
{
    private readonly IOperationRepository _operationRepository = Substitute.For<IOperationRepository>();

    [Fact(DisplayName = "GetOperationByIdQueryHandler Should Return Fail Result When Operation Does Not Exist")]
    public async Task GetOperationByIdQueryHandler_Should_ReturnFailResult_WhenOperationDoesNotExist()
    {
        // Arrange
        var handler = new GetOperationByIdQueryHandler(_operationRepository);

        var operationId = new OperationId(Guid.NewGuid());
        var query = new GetOperationByIdQuery { Id = operationId.Value };

        _operationRepository.GetByIdWithFunctionalityAsync(operationId).Returns((Operation?)null);

        // Act
        Result<OperationDetailedResponse> result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Operations.NotFound");
    }

    [Fact(DisplayName = "GetOperationByIdQueryHandler Should Return Operation When It Exists")]
    public async Task GetOperationByIdQueryHandler_Should_ReturnOperation_WhenItExists()
    {
        // Arrange
        var handler = new GetOperationByIdQueryHandler(_operationRepository);

        var functionalityId = new FunctionalityId(Guid.NewGuid());
        var operationId = new OperationId(Guid.NewGuid());

        var operation = Operation.Create(functionalityId, "Test Operation", "TEST", "test");

        var query = new GetOperationByIdQuery { Id = operationId.Value };

        _operationRepository.GetByIdWithFunctionalityAsync(Arg.Any<OperationId>()).Returns(operation);

        // Act
        Result<OperationDetailedResponse> result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Name.Should().Be("Test Operation");
        result.Value.Code.Should().Be("TEST");
        result.Value.FriendlyId.Should().Be("test");
    }

    [Fact(DisplayName = "GetOperationByIdQueryHandler Should Call Repository With Correct OperationId")]
    public async Task GetOperationByIdQueryHandler_Should_CallRepositoryWithCorrectOperationId()
    {
        // Arrange
        var handler = new GetOperationByIdQueryHandler(_operationRepository);

        var operationId = new OperationId(Guid.NewGuid());
        var query = new GetOperationByIdQuery { Id = operationId.Value };

        _operationRepository.GetByIdWithFunctionalityAsync(Arg.Any<OperationId>()).Returns((Operation?)null);

        // Act
        Result<OperationDetailedResponse> result = await handler.Handle(query, CancellationToken.None);

        // Assert
        await _operationRepository.Received(1).GetByIdWithFunctionalityAsync(Arg.Is<OperationId>(x => x.Value == operationId.Value));
    }
}
