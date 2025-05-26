using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.UseCases.Applications.Commands.UpdateApplication;
using _3ASystem.Application.UseCases.Applications.Responses;
using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Shared;
using FluentAssertions;
using NSubstitute;

namespace _3ASystem.Tests.Application.Application.Commands;

public class UpdateApplicationCommandHandlerTests
{
	private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
	private readonly IAppRepository _appRepository = Substitute.For<IAppRepository>();

	[Fact(DisplayName = "UpdateApplicationCommandHandler Should Return Fail Result When Abbreviation Is Not Unique (Exists in different ID)")]
	public async Task UpdateApplicationCommandHandler_Should_ReturnFailResult_WhenAbbreviationIsNotUnique()
	{
		// Arrange
		var handler = new UpdateApplicationCommandHandler(_unitOfWork, _appRepository);

		var currentApp = App.Create(
			"Test Application",
			"TA",
			"Test Application Description",
			"https://images.icon-icons.com/4231/PNG/32/building_city_icon_263721.png",
			"APL1"
		);

		var existentApp = App.Create(
			"Test Existent Application",
			"TA",
			"Test Existent Application Description",
			"https://images.icon-icons.com/4231/PNG/32/building_city_icon_263721.png",
			"APL1"
		);

		var command = new UpdateApplicationCommand
		{
			Id = currentApp.Id.Value,
			Name = currentApp.Name,
			Abbreviation = currentApp.Abbreviation,
			Description = currentApp.Description,
			IconUrl = currentApp.IconUrl,
			FriendlyId = currentApp.FriendlyId
		};

		_appRepository.GetByIdAsync(Arg.Any<AppId>()).Returns(currentApp);

		//_appRepository.GetByFriendlyIdAsync(Arg.Any<string>()).Returns(existentApp);
		_appRepository.GetByAbbreviationAsync(command.Abbreviation).Returns(existentApp);

		

		// Act
		Result<ApplicationDetailedResponse> result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.IsFailure.Should().BeTrue();
		result.Error.Should().Be(AppErrors.AbbreviationNotUnique);


	}


	[Fact(DisplayName = "UpdateApplicationCommandHandler Should Not Call Unit Of Work When Abbreviation Is Not Unique (Exists)")]
	public async Task UpdateApplicationCommandHandler_Should_NotCallUnitOfWork_WhenAbbreviationIsNotUnique()
	{
		// Arrange
		var handler = new UpdateApplicationCommandHandler(_unitOfWork, _appRepository);

		var currentApp = App.Create(
			"Test Application",
			"TA",
			"Test Application Description",
			"https://images.icon-icons.com/4231/PNG/32/building_city_icon_263721.png",
			"APL1"
		);

		var existentApp = App.Create(
			"Test Existent Application",
			"TA",
			"Test Existent Application Description",
			"https://images.icon-icons.com/4231/PNG/32/building_city_icon_263721.png",
			"APL1"
		);

		var command = new UpdateApplicationCommand
		{
			Id = currentApp.Id.Value,
			Name = currentApp.Name,
			Abbreviation = currentApp.Abbreviation,
			Description = currentApp.Description,
			IconUrl = currentApp.IconUrl,
			FriendlyId = currentApp.FriendlyId
		};

		_appRepository.GetByIdAsync(Arg.Any<AppId>()).Returns(currentApp);
		//_appRepository.GetByFriendlyIdAsync(Arg.Any<string>()).Returns(existentApp);
		_appRepository.GetByAbbreviationAsync(command.Abbreviation).Returns(existentApp);

		// Act
		Result<ApplicationDetailedResponse> result = await handler.Handle(command, CancellationToken.None);

		// Assert
		await _unitOfWork.Received(0).SaveChangesAsync(Arg.Any<CancellationToken>());

	}


	[Fact(DisplayName = "UpdateApplicationCommandHandler Should Call Update Method on Repository When Abbreviation and FriendlyId Not Exist or Exists for Same ID")]
	public async Task UpdateApplicationCommandHandler_Should_CallCreateOnRepository_WhenAbbreviationAndFriendlyIdNotExistOrExistForSameID()
	{
		// Arrange
		var handler = new UpdateApplicationCommandHandler(_unitOfWork, _appRepository);

		var currentApp = App.Create(
			"Test Application",
			"TA",
			"Test Application Description",
			"https://images.icon-icons.com/4231/PNG/32/building_city_icon_263721.png",
			"APL1"
		);

		var existentApp = App.Create(
			"Test Existent Application",
			"TA",
			"Test Existent Application Description",
			"https://images.icon-icons.com/4231/PNG/32/building_city_icon_263721.png",
			"APL1"
		);

		var command = new UpdateApplicationCommand
		{
			Id = currentApp.Id.Value,
			Name = currentApp.Name,
			Abbreviation = currentApp.Abbreviation,
			Description = currentApp.Description,
			IconUrl = currentApp.IconUrl,
			FriendlyId = currentApp.FriendlyId
		};

		_appRepository.GetByIdAsync(Arg.Any<AppId>()).Returns(currentApp);

		App? appNull = null;
		_appRepository.GetByAbbreviationAsync(Arg.Any<string>()).Returns(appNull);
		_appRepository.GetByFriendlyIdAsync(Arg.Any<string>()).Returns(currentApp);

		_appRepository.Update(Arg.Any<App>());

		_unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

		// Act
		Result<ApplicationDetailedResponse> result = await handler.Handle(command, CancellationToken.None);


		// Assert
		_appRepository.Received(1).Update(Arg.Is<App>(app => app.Id.Value == result.Value.Id));

	}


	[Fact(DisplayName = "UpdateApplicationCommandHandler Should Return UpdateApplicationResponse")]
	public async Task UpdateApplicationCommandHandler_Should_ReturnCreateApplicationResponse()
	{
		// Arrange
		var handler = new UpdateApplicationCommandHandler(_unitOfWork, _appRepository);

		var currentApp = App.Create(
			"Test Application",
			"TA",
			"Test Application Description",
			"https://images.icon-icons.com/4231/PNG/32/building_city_icon_263721.png",
			"APL1"
		);

		var command = new UpdateApplicationCommand
		{
			Id = currentApp.Id.Value,
			Name = currentApp.Name,
			Abbreviation = currentApp.Abbreviation,
			Description = currentApp.Description,
			IconUrl = currentApp.IconUrl,
			FriendlyId = currentApp.FriendlyId
		};

		_appRepository.GetByIdAsync(Arg.Any<AppId>()).Returns(currentApp);

		App? appNull = null;
		_appRepository.GetByAbbreviationAsync(Arg.Any<string>()).Returns(appNull);
		_appRepository.GetByFriendlyIdAsync(Arg.Any<string>()).Returns(currentApp);

		_appRepository.Update(Arg.Any<App>());

		_unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

		// Act
		Result<ApplicationDetailedResponse> result = await handler.Handle(command, CancellationToken.None);


		// Assert
		result.IsFailure.Should().BeFalse(); //Assert.False(result.IsFailure);
		result.IsSuccess.Should().BeTrue(); //Assert.True(result.IsSuccess);
		result.Value.Should().NotBeNull(); //Assert.NotNull(result.Value);
		result.Value.Name.Should().BeSameAs(result.Value.Name); //Assert.Equal(command.Name, result.Value.Name);

		//check for repository create method & unit of work save changes async method
		_appRepository.Received(1).Update(Arg.Is<App>(app => app.Id.Value == result.Value.Id));
		await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());

	}


}
