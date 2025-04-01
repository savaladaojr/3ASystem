using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Applications.Commands.CreateApplication;
using _3ASystem.Domain.Data.Repositories;
using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Shared;
using FluentAssertions;
using Moq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace _3ASystem.Tests.Application.Application.Commands;

public class CreateApplicationCommandHandlerTests
{
	Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
	Mock<IAppRepository> _appRepositoryMock = new Mock<IAppRepository>();


	[Fact(DisplayName = "CreateApplicationCommandHandler Should Return Fail Result When Abbreviation Is Not Unique (Exists)")]
	public async Task CreateApplicationCommandHandler_Should_ReturnFailResult_WhenAbbreviationIsNotUnique()
	{
		// Arrange
		var appExistent = App.Create(
			"Test Application Existent",
			"TA",
			"Test Application Existent Description",
			"https://test.com/icon.png"
		);

		_appRepositoryMock.Setup(
			repo => repo.GetByAbbreviationAsync(It.IsAny<string>())
		).ReturnsAsync(appExistent);


		var command = new CreateApplicationCommand
		{
			Name = "Test Application",
			Abbreviation = "TA",
			Description = "Test Application Description",
			IconUrl = "https://test.com/icon.png"
		};

		var app = App.Create(
			command.Name,
			command.Abbreviation,
			command.Description,
			command.IconUrl
		);


		//_appRepositoryMock.Setup(
		//	repo => repo.Create(
		//		It.IsAny<App>()
		//	)
		//).Returns(app);

		//_unitOfWorkMock.Setup(
		//	uow => uow.SaveChangesAsync(
		//		It.IsAny<CancellationToken>()
		//		)
		//).ReturnsAsync(1);

		var handler = new CreateApplicationCommandHandler(_unitOfWorkMock.Object, _appRepositoryMock.Object);

		// Act
		Result<CreateApplicationResponse> result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.IsFailure.Should().BeTrue();
		result.Error.Should().Be(AppErrors.AbbreviationNotUnique);


	}


	[Fact(DisplayName = "CreateApplicationCommandHandler Should Not Call Unit Of Work When Abbreviation Is Not Unique (Exists)")]
	public async Task CreateApplicationCommandHandler_Should_NotCallUnitOfWork_WhenAbbreviationIsNotUnique()
	{
		// Arrange
		var appExistent = App.Create(
			"Test Application Existent",
			"TA",
			"Test Application Existent Description",
			"https://test.com/icon.png"
		);

		_appRepositoryMock.Setup(
			repo => repo.GetByAbbreviationAsync(It.IsAny<string>())
		).ReturnsAsync(appExistent);


		var command = new CreateApplicationCommand
		{
			Name = "Test Application",
			Abbreviation = "TA",
			Description = "Test Application Description",
			IconUrl = "https://test.com/icon.png"
		};

		var handler = new CreateApplicationCommandHandler(_unitOfWorkMock.Object, _appRepositoryMock.Object);


		// Act
		Result<CreateApplicationResponse> result = await handler.Handle(command, CancellationToken.None);


		// Assert
		_unitOfWorkMock.Verify(
			uof => uof.SaveChangesAsync(It.IsAny<CancellationToken>()),
			Times.Never
		);	

	}


	[Fact(DisplayName = "CreateApplicationCommandHandler Should Call Create Method on Repository When Abbreviation not Exist")]
	public async Task CreateApplicationCommandHandler_Should_CallCreateOnRepository_WhenAbbreviationNotExist()
	{
		// Arrange
		var command = new CreateApplicationCommand
		{
			Name = "Test Application",
			Abbreviation = "TA",
			Description = "Test Application Description",
			IconUrl = "https://test.com/icon.png"
		};

		var app = App.Create(
			command.Name,
			command.Abbreviation,
			command.Description,
			command.IconUrl
		);

		App? appNull = null;
		_appRepositoryMock.Setup(
			repo => repo.GetByAbbreviationAsync(It.IsAny<string>())
		).ReturnsAsync(appNull);

		_appRepositoryMock.Setup(
			repo => repo.Create(
				It.IsAny<App>()
			)
		).Returns(app);

		_unitOfWorkMock.Setup(
			uow => uow.SaveChangesAsync(
				It.IsAny<CancellationToken>()
				)
		).ReturnsAsync(1);

		var handler = new CreateApplicationCommandHandler(_unitOfWorkMock.Object, _appRepositoryMock.Object);


		// Act
		Result<CreateApplicationResponse> result = await handler.Handle(command, CancellationToken.None);


		// Assert
		_appRepositoryMock.Verify(
			repo => repo.Create(
				It.Is<App>(app => app.Id.Value == result.Value.Id)
			),
			Times.Once
		);

	}


	[Fact(DisplayName = "CreateApplicationCommandHandler Should Return CreateApplicationResponse")]
	public async Task CreateApplicationCommandHandler_Should_ReturnCreateApplicationResponse()
	{
		// Arrange
		var command = new CreateApplicationCommand
		{
			Name = "Test Application",
			Abbreviation = "TA",
			Description = "Test Application Description",
			IconUrl = "https://test.com/icon.png"
		};

		var app = App.Create(
			command.Name,
			command.Abbreviation,
			command.Description,
			command.IconUrl
		);

		_appRepositoryMock.Setup(
			repo => repo.Create(
				It.IsAny<App>()
			)
		).Returns(app);

		_unitOfWorkMock.Setup(
			uow => uow.SaveChangesAsync(
				It.IsAny<CancellationToken>()
				)
		).ReturnsAsync(1);

		var handler = new CreateApplicationCommandHandler(_unitOfWorkMock.Object, _appRepositoryMock.Object);


		// Act
		Result<CreateApplicationResponse> result = await handler.Handle(command, CancellationToken.None);


		// Assert

		//check for repository create method
		_appRepositoryMock.Verify(
			repo => repo.Create(
				It.Is<App>(app => app.Id.Value == result.Value.Id)
			),
			Times.Once
		);


		/* //Without Fluent Assertion
		Assert.False(result.IsFailure);
		Assert.True(result.IsSuccess);
		Assert.NotNull(result.Value);
		Assert.Equal(command.Name, result.Value.Name);
		*/

		result.IsFailure.Should().BeFalse();
		result.IsSuccess.Should().BeTrue();
		result.Value.Should().NotBeNull();
		result.Value.Name.Should().BeSameAs(result.Value.Name);
	}


}
