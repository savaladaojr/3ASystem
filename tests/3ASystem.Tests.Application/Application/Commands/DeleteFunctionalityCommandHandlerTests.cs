using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.UseCases.Functionalities.Commands.DeleteFunctionality;
using _3ASystem.Domain.Entities.Functionalities;
using _3ASystem.Domain.Entities.Modules;
using _3ASystem.Domain.Entities.Operations;
using FluentAssertions;
using NSubstitute;

namespace _3ASystem.Tests.Application.Application.Commands;

public class DeleteFunctionalityCommandHandlerTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IFunctionalityRepository _functionalityRepository = Substitute.For<IFunctionalityRepository>();
    private readonly IOperationRepository _operationRepository = Substitute.For<IOperationRepository>();

    [Fact]
    public async Task Handle_ShouldRejectDeletion_WhenFunctionalityHasOperations()
    {
        var functionality = CreateFunctionality();
        _functionalityRepository.GetByIdAsync(functionality.Id).Returns(functionality);
        _operationRepository.ExistsByFunctionalityIdAsync(functionality.Id).Returns(true);

        var handler = CreateHandler();
        var result = await handler.Handle(
            new DeleteFunctionalityCommand(functionality.Id.Value),
            CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(OperationErrors.FunctionalityHasOperations(functionality.Id).Code);
        _functionalityRepository.DidNotReceive().Delete(functionality.Id);
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldDeleteFunctionality_WhenItHasNoOperations()
    {
        var functionality = CreateFunctionality();
        _functionalityRepository.GetByIdAsync(functionality.Id).Returns(functionality);
        _operationRepository.ExistsByFunctionalityIdAsync(functionality.Id).Returns(false);

        var handler = CreateHandler();
        var result = await handler.Handle(
            new DeleteFunctionalityCommand(functionality.Id.Value),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _functionalityRepository.Received(1).Delete(functionality.Id);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    private DeleteFunctionalityCommandHandle CreateHandler() =>
        new(_unitOfWork, _functionalityRepository, _operationRepository);

    private static Functionality CreateFunctionality() =>
        Functionality.Create(
            new ModuleId(Guid.NewGuid()),
            "Accounts",
            "ACC",
            "/accounts",
            string.Empty,
            string.Empty,
            "accounts",
            true);
}
