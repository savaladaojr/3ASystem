using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Applications.Responses;
using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Applications.Commands.CreateApplication;


public sealed class CreateApplicationCommandHandler : ICommandHandler<CreateApplicationCommand, ApplicationDetailedResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IAppRepository _appRepository;

	public CreateApplicationCommandHandler(IUnitOfWork unitOfWork, IAppRepository appRepository)
	{
		_unitOfWork = unitOfWork;
		_appRepository = appRepository;
	}

	public async Task<Result<ApplicationDetailedResponse>> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
	{
		//Check if the Abbreviation is unique
		var appAbbreviation = await _appRepository.GetByAbbreviationAsync(request.Abbreviation);
		if (appAbbreviation is not null)
			return Result.Failure<ApplicationDetailedResponse>(AppErrors.AbbreviationNotUnique);

		//Check if the FriendlyID is unique
		var appFriendlyId = await _appRepository.GetByFriendlyIdAsync(request.FriendlyId);
		if (appFriendlyId is not null)
			return Result.Failure<ApplicationDetailedResponse>(AppErrors.FriendlyIdNotUnique);


		var app = App.Create(request.Name, request.Abbreviation, request.Description, request.IconUrl, request.FriendlyId);

		app.Raise(new AppCreatedDomainEvent(app.Id));

		_appRepository.Create(app);

		await _unitOfWork.SaveChangesAsync(cancellationToken);

		//Return created app
		return app.ToApplicationDetailedResponse();
	}

}
