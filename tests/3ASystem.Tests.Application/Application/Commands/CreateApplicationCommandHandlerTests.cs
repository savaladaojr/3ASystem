using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.UseCases.Applications.Commands.CreateApplication;
using _3ASystem.Application.UseCases.Applications.Responses;
using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Shared;
using FluentAssertions;
using NSubstitute;

namespace _3ASystem.Tests.Application.Application.Commands;

public class CreateApplicationCommandHandlerTests
{
	private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
	private readonly IAppRepository _appRepository = Substitute.For<IAppRepository>();

	[Fact(DisplayName = "CreateApplicationCommandHandler Should Return Fail Result When Abbreviation Is Not Unique (Exists)")]
	public async Task CreateApplicationCommandHandler_Should_ReturnFailResult_WhenAbbreviationIsNotUnique()
	{
		// Arrange
		var handler = new CreateApplicationCommandHandler(_unitOfWork, _appRepository);

		var command = new CreateApplicationCommand
		{
			Name = "Test Application",
			Abbreviation = "TA",
			Description = "Test Application Description",
			IconUrl = "https://test.com/icon.png",
			FriendlyId = "APL1"
		};


		var existentApp= App.Create(
			command.Name,
			command.Abbreviation,
			command.Description,
			command.IconUrl,
			command.FriendlyId
		);

		_appRepository.GetByAbbreviationAsync(command.Abbreviation).Returns(existentApp);	

		// Act
		Result<ApplicationResponse> result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.IsFailure.Should().BeTrue();
		result.Error.Should().Be(AppErrors.AbbreviationNotUnique);


	}


	[Fact(DisplayName = "CreateApplicationCommandHandler Should Not Call Unit Of Work When Abbreviation Is Not Unique (Exists)")]
	public async Task CreateApplicationCommandHandler_Should_NotCallUnitOfWork_WhenAbbreviationIsNotUnique()
	{
		// Arrange
		var handler = new CreateApplicationCommandHandler(_unitOfWork, _appRepository);

		var appExistent = App.Create(
			"Test Application Existent",
			"TA",
			"Test Application Existent Description",
			"https://test.com/icon.png",
			"APL1"
		);

		var command = new CreateApplicationCommand
		{
			Name = "Test Application",
			Abbreviation = "TA",
			Description = "Test Application Description",
			IconUrl = "https://test.com/icon.png",
			FriendlyId = "APL1"
		};

		_appRepository.GetByAbbreviationAsync(command.Abbreviation).Returns(appExistent);

		// Act
		Result<ApplicationResponse> result = await handler.Handle(command, CancellationToken.None);

		// Assert
		await _unitOfWork.Received(0).SaveChangesAsync(Arg.Any<CancellationToken>());

	}


	[Fact(DisplayName = "CreateApplicationCommandHandler Should Call Create Method on Repository When Abbreviation not Exist")]
	public async Task CreateApplicationCommandHandler_Should_CallCreateOnRepository_WhenAbbreviationNotExist()
	{
		// Arrange
		var handler = new CreateApplicationCommandHandler(_unitOfWork, _appRepository);

		var command = new CreateApplicationCommand
		{
			Name = "Test Application",
			Abbreviation = "TA",
			Description = "Test Application Description",
			IconUrl = "https://test.com/icon.png",
			FriendlyId = "APL1"
		};

		var app = App.Create(
			command.Name,
			command.Abbreviation,
			command.Description,
			command.IconUrl,
			command.FriendlyId
		);


		App? appNull = null;
		_appRepository.GetByAbbreviationAsync(Arg.Any<string>()).Returns(appNull);

		_appRepository.Create(app).Returns(app);

		_unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

		// Act
		Result<ApplicationResponse> result = await handler.Handle(command, CancellationToken.None);


		// Assert
		_appRepository.Received(1).Create(Arg.Any<App>());

	}


	[Fact(DisplayName = "CreateApplicationCommandHandler Should Return ApplicationResponse")]
	public async Task CreateApplicationCommandHandler_Should_ReturnCreateApplicationResponse()
	{
		// Arrange
		var handler = new CreateApplicationCommandHandler(_unitOfWork, _appRepository);

		var command = new CreateApplicationCommand
		{
			Name = "Test Application",
			Abbreviation = "TA",
			Description = "Test Application Description",
			IconUrl = "https://test.com/icon.png",
			FriendlyId = "APL1"
		};

		var app = App.Create(
			command.Name,
			command.Abbreviation,
			command.Description,
			command.IconUrl,
			command.FriendlyId
		);

		App? appNull = null;
		_appRepository.GetByAbbreviationAsync(Arg.Any<string>()).Returns(appNull);

		_appRepository.Create(app).Returns(app);

		_unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

		// Act
		Result<ApplicationResponse> result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.IsFailure.Should().BeFalse(); //Assert.False(result.IsFailure);
		result.IsSuccess.Should().BeTrue(); //Assert.True(result.IsSuccess);
		result.Value.Should().NotBeNull(); //Assert.NotNull(result.Value);
		result.Value.Name.Should().BeSameAs(result.Value.Name); //Assert.Equal(command.Name, result.Value.Name);

		//check for repository create method & unit of work save changes async method
		_appRepository.Received(1).Create(Arg.Is<App>(app => app.Id.Value == result.Value.Id));
		await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());

	}


}
