using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.UseCases.Operations.Commands.EnableDisableOperation;
using _3ASystem.Domain.Entities.Functionalities;
using _3ASystem.Domain.Entities.Operations;
using FluentAssertions;
using NSubstitute;

namespace _3ASystem.Tests.Application.Application.Commands;

public class EnableDisableOperationCommandHandlerTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IOperationRepository _operationRepository = Substitute.For<IOperationRepository>();

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenOperationDoesNotExist()
    {
        var operationId = new OperationId(Guid.NewGuid());
        _operationRepository.GetByIdAsync(operationId).Returns((Operation?)null);

        var handler = new EnableDisableOperationCommandHandler(_operationRepository, _unitOfWork);
        var result = await handler.Handle(
            new EnableDisableOperationCommand { Id = operationId.Value, IsActive = true },
            CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Operations.NotFound");
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Handle_ShouldSetActiveStateAndSave(bool isActive)
    {
        var functionalityId = new FunctionalityId(Guid.NewGuid());
        var operation = Operation.Create(functionalityId, "Read", "ACC.Read", "accounts-Read");
        _operationRepository.GetByIdAsync(operation.Id).Returns(operation);

        var handler = new EnableDisableOperationCommandHandler(_operationRepository, _unitOfWork);
        var result = await handler.Handle(
            new EnableDisableOperationCommand { Id = operation.Id.Value, IsActive = isActive },
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.IsActive.Should().Be(isActive);
        _operationRepository.Received(1).Update(operation);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
