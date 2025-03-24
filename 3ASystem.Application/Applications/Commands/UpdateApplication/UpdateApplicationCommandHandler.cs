using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Domain.Data.Repositories;
using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Shared;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace _3ASystem.Application.Applications.Commands.UpdateApplication;

internal sealed class UpdateApplicationCommandHandler : ICommandHandler<UpdateApplicationCommand, UpdateApplicationResponse>
{
	private readonly IAppRepository _appRepository;
	private readonly IUnitOfWork _unitOfWork;

	public UpdateApplicationCommandHandler(IAppRepository appRepository,IUnitOfWork unitOfWork)
	{
		_appRepository = appRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<UpdateApplicationResponse>> Handle(UpdateApplicationCommand request, CancellationToken cancellationToken)
	{
		var appId = new AppId(request.Id);
		var app = await _appRepository.GetByIdAsync(appId);

		if (app is null)
			return Result.Failure<UpdateApplicationResponse>(AppErrors.NotFound(appId));

		app.Update(request.Name, request.Abbreviation, request.Description, request.IconUrl, request.IsActive);

		app.Raise(new AppUpdatedDomainEvent(app.Id));

		_appRepository.Update(app);
		await _unitOfWork.SaveChangesAsync(cancellationToken);

		var finalResult = new UpdateApplicationResponse
		{
			Id = app.Id.Value,
			Name = app.Name,
			Abbreviation = app.Abbreviation,
			Description = app.Description,
			IconUrl = app.IconUrl,
			Hash = app.Hash,
			IsActive = app.IsActive
		};

		return finalResult;
	}

}
