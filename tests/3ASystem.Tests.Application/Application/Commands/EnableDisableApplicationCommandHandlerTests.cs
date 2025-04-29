using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.UseCases.Applications.Commands.EnableDisableApplication;
using _3ASystem.Application.UseCases.Applications.Responses;
using _3ASystem.Domain.Data.Repositories;
using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Shared;
using FluentAssertions;
using NSubstitute;

namespace _3ASystem.Tests.Application.Application.Commands;

public class EnableDisableApplicationCommandHandlerTests
{
	private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
	private readonly IAppRepository _appRepository = Substitute.For<IAppRepository>();

	[Fact(DisplayName = "EnableDisableApplicationCommandHandler Should Return Fail Result When Record Not Found (Not Exists)")]
	public async Task EnableDisableApplicationCommandValidatorHandler_Should_ReturnFailResult_WhenRecordNotFound()
	{
		// Arrange
		var handler = new EnableDisableApplicationCommandHandler(_unitOfWork, _appRepository);

		var existentApp = App.Create(
			"Test Application",
			"TA",
			"Test Application Description",
			"https://test.com/icon.png",
			"APL1"
		);

		var appId = new AppId(Guid.NewGuid());
		var command = new EnableDisableApplicationCommand() { Id = Guid.NewGuid() };

		App? appNull = null;

		_appRepository.GetByIdAsync(Arg.Any<AppId>()).Returns(appNull); //record not found

		// Act
		Result result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.IsFailure.Should().BeTrue();
		result.Error.Type.Should().Be(ErrorType.NotFound);

	}

	[Fact(DisplayName = "EnableDisableApplicationCommandHandler Should Process When Record was Found (Exists)")]
	public async Task DeleteApplicationCommandHandler_Should_Process_WhenRecordNotFound()
	{
		// Arrange
		var handler = new EnableDisableApplicationCommandHandler(_unitOfWork, _appRepository);

		var existentApp = App.Create(
			"Test Application",
			"TA",
			"Test Application Description",
			"https://test.com/icon.png",
			"APL1"
		);

		var appId = existentApp.Id;
		var command = new EnableDisableApplicationCommand() { Id = appId.Value };

		App? appNull = null;

		_appRepository.GetByIdAsync(Arg.Any<AppId>()).Returns(existentApp); //record found

		// Act
		Result<ApplicationResponse> result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.IsFailure.Should().BeFalse(); //Assert.False(result.IsFailure);
		result.IsSuccess.Should().BeTrue(); //Assert.True(result.IsSuccess);
		result.Value.Should().NotBeNull(); //Assert.NotNull(result.Value);	
		result.Value.IsActive.Should().BeFalse(); //Assert.False(result.Value.IsActive);

		//check for repository create method & unit of work save changes async method
		_appRepository.Received(1).Update(Arg.Is<App>(app => app.Id.Value == result.Value.Id));
		await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());

	}




}
