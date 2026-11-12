using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.UseCases.Operations.Commands.AddBasicOperations;
using _3ASystem.Domain.Entities.Functionalities;
using _3ASystem.Domain.Entities.Modules;
using _3ASystem.Domain.Entities.Operations;
using FluentAssertions;
using NSubstitute;

namespace _3ASystem.Tests.Application.Application.Commands;

public class AddBasicOperationsCommandHandlerTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IOperationRepository _operationRepository = Substitute.For<IOperationRepository>();
    private readonly IFunctionalityRepository _functionalityRepository = Substitute.For<IFunctionalityRepository>();

    [Fact]
    public async Task Handle_ShouldCreateFourStandardOperations_WhenNoneExist()
    {
        var functionality = CreateFunctionality();
        _functionalityRepository.GetByIdAsync(functionality.Id).Returns(functionality);
        _operationRepository.GetByCodeAsync(Arg.Any<string>()).Returns((Operation?)null);
        _operationRepository.GetByFriendlyIdAsync(Arg.Any<string>()).Returns((Operation?)null);

        var handler = CreateHandler();
        var result = await handler.Handle(
            new AddBasicOperationsCommand { FunctionalityId = functionality.Id.Value },
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Select(operation => operation.Name).Should().ContainInOrder(
            "Accounts View", "Accounts Create", "Accounts Update", "Accounts Delete");
        result.Value.Should().HaveCount(4);
        _operationRepository.Received(4).Create(Arg.Any<Operation>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnExistingOperationsWithoutSaving_WhenAllAlreadyExist()
    {
        var functionality = CreateFunctionality();
        var definitions = new[]
        {
            ("View", "View"),
            ("Create", "Create"),
            ("Update", "Update"),
            ("Delete", "Delete")
        };
        var operations = definitions.Select(definition => Operation.Create(
            functionality.Id,
            $"{functionality.Name} {definition.Item1}",
            $"{functionality.Abbreviation}.{definition.Item2}",
            $"{functionality.FriendlyId}-{definition.Item2}")).ToArray();

        _functionalityRepository.GetByIdAsync(functionality.Id).Returns(functionality);
        _operationRepository.GetByCodeAsync(Arg.Any<string>())
            .Returns(call => operations.Single(operation => operation.Code == call.Arg<string>()));
        _operationRepository.GetByFriendlyIdAsync(Arg.Any<string>())
            .Returns(call => operations.Single(operation => operation.FriendlyId == call.Arg<string>()));

        var handler = CreateHandler();
        var command = new AddBasicOperationsCommand { FunctionalityId = functionality.Id.Value };
        var firstResult = await handler.Handle(command, CancellationToken.None);
        var secondResult = await handler.Handle(command, CancellationToken.None);

        firstResult.Value.Should().HaveCount(4);
        secondResult.Value.Should().HaveCount(4);
        _operationRepository.DidNotReceive().Create(Arg.Any<Operation>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldCreateOnlyMissingOperations_WhenSomeAlreadyExist()
    {
        var functionality = CreateFunctionality();
        var existingView = Operation.Create(
            functionality.Id,
            "Accounts View",
            "ACC.View",
            "accounts-View");

        _functionalityRepository.GetByIdAsync(functionality.Id).Returns(functionality);
        _operationRepository.GetByCodeAsync(Arg.Any<string>()).Returns((Operation?)null);
        _operationRepository.GetByFriendlyIdAsync(Arg.Any<string>()).Returns((Operation?)null);
        _operationRepository.GetByCodeAsync("ACC.View").Returns(existingView);
        _operationRepository.GetByFriendlyIdAsync("accounts-View").Returns(existingView);

        var handler = CreateHandler();
        var result = await handler.Handle(
            new AddBasicOperationsCommand { FunctionalityId = functionality.Id.Value },
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(4);
        result.Value.Select(operation => operation.Name).Should().Contain("Accounts View");
        _operationRepository.Received(3).Create(Arg.Any<Operation>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldFailWithoutAddingAnyOperations_WhenAStandardCodeConflicts()
    {
        var functionality = CreateFunctionality();
        var conflictingOperation = Operation.Create(
            functionality.Id,
            "Existing operation",
            "ACC.View",
            "different-friendly-id");

        _functionalityRepository.GetByIdAsync(functionality.Id).Returns(functionality);
        _operationRepository.GetByCodeAsync("ACC.View").Returns(conflictingOperation);
        _operationRepository.GetByCodeAsync(Arg.Is<string>(code => code != "ACC.View")).Returns((Operation?)null);
        _operationRepository.GetByFriendlyIdAsync(Arg.Any<string>()).Returns((Operation?)null);

        var handler = CreateHandler();
        var result = await handler.Handle(
            new AddBasicOperationsCommand { FunctionalityId = functionality.Id.Value },
            CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(OperationErrors.CodeNotUnique.Code);
        _operationRepository.DidNotReceive().Create(Arg.Any<Operation>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    private AddBasicOperationsCommandHandler CreateHandler() =>
        new(_operationRepository, _functionalityRepository, _unitOfWork);

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
