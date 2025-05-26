using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Applications.Responses;
using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Shared;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace _3ASystem.Application.UseCases.Applications.Commands.UpdateApplication;

public sealed class UpdateApplicationCommandHandler : ICommandHandler<UpdateApplicationCommand, ApplicationDetailedResponse>
{
	private readonly IAppRepository _appRepository;
	private readonly IUnitOfWork _unitOfWork;

	public UpdateApplicationCommandHandler(IUnitOfWork unitOfWork, IAppRepository appRepository)
	{
		_appRepository = appRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<ApplicationDetailedResponse>> Handle(UpdateApplicationCommand request, CancellationToken cancellationToken)
	{
		var appId = new AppId(request.Id);
		var app = await _appRepository.GetByIdAsync(appId);

		if (app is null)
			return Result.Failure<ApplicationDetailedResponse>(AppErrors.NotFound(appId));

		//Check if the Abbreviation is unique
		var appAbbreviation = await _appRepository.GetByAbbreviationAsync(request.Abbreviation);
		if ( appAbbreviation is not null && appAbbreviation.Id != app.Id)
			return Result.Failure<ApplicationDetailedResponse>(AppErrors.AbbreviationNotUnique);


		//Check if the FriendlyID is unique
		var appFriendlyId = await _appRepository.GetByFriendlyIdAsync(request.FriendlyId);
		if (appFriendlyId is not null && appFriendlyId.Id != app.Id)
			return Result.Failure<ApplicationDetailedResponse>(AppErrors.FriendlyIdNotUnique);

		app.Update(request.Name, request.Abbreviation, request.Description, request.IconUrl, request.FriendlyId);

		app.Raise(new AppUpdatedDomainEvent(app.Id));

		_appRepository.Update(app);
		await _unitOfWork.SaveChangesAsync(cancellationToken);

		var finalResult = new ApplicationDetailedResponse
		{
			Id = app.Id.Value,
			Name = app.Name,
			Abbreviation = app.Abbreviation,
			Description = app.Description,
			IconUrl = app.IconUrl,
			Hash = app.Hash,
			IsActive = app.IsActive,
			FriendlyId = app.FriendlyId,
			CreatedAt = app.CreatedAt,
			LastUpdatedAt = app.LastUpdatedAt
		};

		return finalResult;
	}

}
