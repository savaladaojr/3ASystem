using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.UseCases.Operations.Queries.GetOperationsByFunctionality;
using _3ASystem.Application.UseCases.Operations.Responses;
using _3ASystem.Domain.Entities.Functionalities;
using _3ASystem.Domain.Entities.Operations;
using _3ASystem.Domain.Shared;
using FluentAssertions;
using NSubstitute;

namespace _3ASystem.Tests.Application.Application.Queries;

public class GetOperationsByFunctionalityQueryHandlerTests
{
    private readonly IOperationRepository _operationRepository = Substitute.For<IOperationRepository>();

    [Fact(DisplayName = "GetOperationsByFunctionalityQueryHandler Should Return Empty List When No Operations Exist")]
    public async Task GetOperationsByFunctionalityQueryHandler_Should_ReturnEmptyList_WhenNoOperationsExist()
    {
        // Arrange
        var handler = new GetOperationsByFunctionalityHandler(_operationRepository);

        var functionalityId = new FunctionalityId(Guid.NewGuid());
        var query = new GetOperationsByFunctionalityQuery { FunctionalityId = functionalityId.Value };

        _operationRepository.GetByFunctionalityIdAsync(functionalityId).Returns(new List<Operation>());

        // Act
        Result<List<OperationResponse>> result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeEmpty();
    }

    [Fact(DisplayName = "GetOperationsByFunctionalityQueryHandler Should Return List Of Operations When They Exist")]
    public async Task GetOperationsByFunctionalityQueryHandler_Should_ReturnListOfOperations_WhenTheyExist()
    {
        // Arrange
        var handler = new GetOperationsByFunctionalityHandler(_operationRepository);

        var functionalityId = new FunctionalityId(Guid.NewGuid());
        var query = new GetOperationsByFunctionalityQuery { FunctionalityId = functionalityId.Value };

        var operations = new List<Operation>
        {
            Operation.Create(functionalityId, "Operation 1", "OP001", "op1"),
            Operation.Create(functionalityId, "Operation 2", "OP002", "op2"),
            Operation.Create(functionalityId, "Operation 3", "OP003", "op3")
        };

        _operationRepository.GetByFunctionalityIdAsync(functionalityId).Returns(operations);

        // Act
        Result<List<OperationResponse>> result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(3);
        result.Value[0].Name.Should().Be("Operation 1");
        result.Value[1].Name.Should().Be("Operation 2");
        result.Value[2].Name.Should().Be("Operation 3");
    }

    [Fact(DisplayName = "GetOperationsByFunctionalityQueryHandler Should Call Repository With Correct FunctionalityId")]
    public async Task GetOperationsByFunctionalityQueryHandler_Should_CallRepositoryWithCorrectFunctionalityId()
    {
        // Arrange
        var handler = new GetOperationsByFunctionalityHandler(_operationRepository);

        var functionalityId = new FunctionalityId(Guid.NewGuid());
        var query = new GetOperationsByFunctionalityQuery { FunctionalityId = functionalityId.Value };

        _operationRepository.GetByFunctionalityIdAsync(Arg.Any<FunctionalityId>()).Returns(new List<Operation>());

        // Act
        Result<List<OperationResponse>> result = await handler.Handle(query, CancellationToken.None);

        // Assert
        await _operationRepository.Received(1).GetByFunctionalityIdAsync(Arg.Is<FunctionalityId>(x => x.Value == functionalityId.Value));
    }
}
