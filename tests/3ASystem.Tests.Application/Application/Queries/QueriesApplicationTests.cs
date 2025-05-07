using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.UseCases.Applications.Queries.GetApplicationByAbbreviation;
using _3ASystem.Application.UseCases.Applications.Queries.GetApplicationByFriendlyId;
using _3ASystem.Application.UseCases.Applications.Queries.GetApplicationByHash;
using _3ASystem.Application.UseCases.Applications.Queries.GetApplicationById;
using _3ASystem.Application.UseCases.Applications.Queries.GetApplications;
using _3ASystem.Application.UseCases.Applications.Responses;
using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Shared;
using AutoFixture;
using FluentAssertions;
using NSubstitute;

namespace _3ASystem.Tests.Application.Application.Queries;

public class QueriesApplicationTests
{
	private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
	private readonly IAppRepository _appRepository = Substitute.For<IAppRepository>();

	IEnumerable<App> _listOfApps = new List<App>();

	public QueriesApplicationTests()
	{
		var fixture = new Fixture();
		fixture.Behaviors.Add(new OmitOnRecursionBehavior());
		_listOfApps = fixture.CreateMany<App>(100).ToList();
	}


	/*
	stubEmailService.Send(string.Empty).ReturnsForAnyArgs (callInfo =>
    {
        // where 0 is the index of the parameter to the method
        var email = callInfo.ArgAt<string>(0);
        return email.Length < 10;
    });
    */

	[Fact(DisplayName = "GetApplicationsHandler Should Return a List Of Application Compact Response When It Is Called")]
	public async Task GetApplicationsHandler_Should_ReturnListOfApplicationCResponseWhenItIsCalled()
	{
		// Arrange
		var handler = new GetApplicationsHandler(_appRepository);
		_appRepository.GetAllAsync().Returns(_listOfApps);

		//_appRepository.GetByAbbreviationAsync(Arg.Any<string>()).Returns(appNull);

		var command = new GetApplicationsQuery();
		// Act
		Result<List<ApplicationCResponse>> result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.IsFailure.Should().BeFalse(); //Assert.False(result.IsFailure);
		result.IsSuccess.Should().BeTrue(); //Assert.True(result.IsSuccess);
		result.Value.Should().NotBeNull(); //Assert.NotNull(result.Value);
		result.Value.Count().Should().Be(100); //Assert.Equal(result.Value.Count(), 100);

		//check for repository create method & unit of work save changes async method
		await _appRepository.Received(1).GetAllAsync();

	}

	[Fact(DisplayName = "GetApplicationByIdHandler Should Return an Application Response")]
	public async Task GetApplicationByIdHandler_Should_ReturnAnApplicationResponseWhenItIsCalled()
	{
		// Arrange
		var handler = new GetApplicationByIdHandler(_appRepository);
		_appRepository.GetByIdAsync(Arg.Any<AppId>()).ReturnsForAnyArgs(_listOfApps.FirstOrDefault());

		var command = new GetApplicationByIdQuery() { Id = Guid.NewGuid() };
		// Act
		Result<ApplicationResponse> result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.IsFailure.Should().BeFalse(); //Assert.False(result.IsFailure);
		result.IsSuccess.Should().BeTrue(); //Assert.True(result.IsSuccess);
		result.Value.Should().NotBeNull(); //Assert.NotNull(result.Value);
		result.Value.Should().BeOfType<ApplicationResponse>(); //Assert.IsType<List<ApplicationResponse>>(result.Value);

		//check for repository create method & unit of work save changes async method
		await _appRepository.Received(1).GetByIdAsync(Arg.Any<AppId>());

	}

	[Fact(DisplayName = "GetApplicationByHashHandler Should Return an Application Response")]
	public async Task GetApplicationByHashHandler_Should_ReturnAnApplicationResponseWhenItIsCalled()
	{
		// Arrange
		var handler = new GetApplicationByHashHandler(_appRepository);
		_appRepository.GetByHashAsync(Arg.Any<Guid>()).ReturnsForAnyArgs(_listOfApps.FirstOrDefault());

		var command = new GetApplicationByHashQuery() { Hash = Guid.NewGuid() };
		// Act
		Result<ApplicationResponse> result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.IsFailure.Should().BeFalse(); //Assert.False(result.IsFailure);
		result.IsSuccess.Should().BeTrue(); //Assert.True(result.IsSuccess);
		result.Value.Should().NotBeNull(); //Assert.NotNull(result.Value);
		result.Value.Should().BeOfType<ApplicationResponse>(); //Assert.IsType<List<ApplicationResponse>>(result.Value);

		//check for repository create method & unit of work save changes async method
		await _appRepository.Received(1).GetByHashAsync(Arg.Any<Guid>());

	}

	[Fact(DisplayName = "GetApplicationByFriendlyIdHandler Should Return an Application Response")]
	public async Task GetApplicationByFriendlyIdHandler_Should_ReturnAnApplicationResponseWhenItIsCalled()
	{
		// Arrange
		var handler = new GetApplicationByFriendlyIdHandler(_appRepository);
		_appRepository.GetByFriendlyIdAsync(Arg.Any<string>()).ReturnsForAnyArgs(_listOfApps.FirstOrDefault());

		var command = new GetApplicationByFriendlyIdQuery() { FriendlyId = "AnyFriendlyId" };
		// Act
		Result<ApplicationResponse> result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.IsFailure.Should().BeFalse(); //Assert.False(result.IsFailure);
		result.IsSuccess.Should().BeTrue(); //Assert.True(result.IsSuccess);
		result.Value.Should().NotBeNull(); //Assert.NotNull(result.Value);
		result.Value.Should().BeOfType<ApplicationResponse>(); //Assert.IsType<List<ApplicationResponse>>(result.Value);

		//check for repository create method & unit of work save changes async method
		await _appRepository.Received(1).GetByFriendlyIdAsync(Arg.Any<string>());

	}

	[Fact(DisplayName = "GetApplicationByAbbreviationHandler Should Return an Application Response")]
	public async Task GetApplicationByAbbreviationHandler_Should_ReturnAnApplicationResponseWhenItIsCalled()
	{
		// Arrange
		var handler = new GetApplicationByAbbreviationHandler(_appRepository);
		_appRepository.GetByAbbreviationAsync(Arg.Any<string>()).ReturnsForAnyArgs(_listOfApps.FirstOrDefault());

		var command = new GetApplicationByAbbreviationQuery() { Abbreviation = "AnyAbbreviation" };
		// Act
		Result<ApplicationResponse> result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.IsFailure.Should().BeFalse(); //Assert.False(result.IsFailure);
		result.IsSuccess.Should().BeTrue(); //Assert.True(result.IsSuccess);
		result.Value.Should().NotBeNull(); //Assert.NotNull(result.Value);
		result.Value.Should().BeOfType<ApplicationResponse>(); //Assert.IsType<List<ApplicationResponse>>(result.Value);

		//check for repository create method & unit of work save changes async method
		await _appRepository.Received(1).GetByAbbreviationAsync(Arg.Any<string>());

	}


}
