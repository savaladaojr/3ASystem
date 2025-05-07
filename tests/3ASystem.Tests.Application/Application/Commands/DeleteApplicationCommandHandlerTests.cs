using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.UseCases.Applications.Commands.DeleteApplication;
using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Shared;
using FluentAssertions;
using NSubstitute;

namespace _3ASystem.Tests.Application.Application.Commands;

public class DeleteApplicationCommandHandlerTests
{
	private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
	private readonly IAppRepository _appRepository = Substitute.For<IAppRepository>();

	[Fact(DisplayName = "DeleteApplicationCommandHandler Should Return Fail Result When Record Not Found (Not Exists)")]
	public async Task DeleteApplicationCommandHandler_Should_ReturnFailResult_WhenRecordNotFound()
	{
		// Arrange
		var handler = new DeleteApplicationCommandHandle(_unitOfWork, _appRepository);

		var existentApp = App.Create(
			"Test Application",
			"TA",
			"Test Application Description",
			"https://test.com/icon.png",
			"APL1"
		);

		var appId = new AppId(Guid.NewGuid());
		var command = new DeleteApplicationCommand(Guid.NewGuid());

		App? appNull = null;

		_appRepository.GetByIdAsync(Arg.Any<AppId>()).Returns(appNull); //record not found

		// Act
		Result result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.IsFailure.Should().BeTrue();
		result.Error.Type.Should().Be(ErrorType.NotFound);

	}

	[Fact(DisplayName = "DeleteApplicationCommandHandler Should Process When Record was Found (Exists)")]
	public async Task DeleteApplicationCommandHandler_Should_Process_WhenRecordNotFound()
	{
		// Arrange
		var handler = new DeleteApplicationCommandHandle(_unitOfWork, _appRepository);

		var existentApp = App.Create(
			"Test Application",
			"TA",
			"Test Application Description",
			"https://test.com/icon.png",
			"APL1"
		);

		var appId = existentApp.Id;
		var command = new DeleteApplicationCommand(appId.Value);

		//App? appNull = null;

		_appRepository.GetByIdAsync(Arg.Any<AppId>()).Returns(existentApp); //record found

		// Act
		Result result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.IsFailure.Should().BeFalse(); //Assert.False(result.IsFailure);
		result.IsSuccess.Should().BeTrue(); //Assert.True(result.IsSuccess);

		//check for repository create method & unit of work save changes async method
		_appRepository.Received(1).Delete(Arg.Is<AppId>(app => app.Value == appId.Value));
		await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());

	}




}
