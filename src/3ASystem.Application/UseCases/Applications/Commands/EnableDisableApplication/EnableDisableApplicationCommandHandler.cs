using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Applications.Responses;
using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Applications.Commands.EnableDisableApplication;

public sealed class EnableDisableApplicationCommandHandler : ICommandHandler<EnableDisableApplicationCommand, ApplicationDetailedResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IAppRepository _appRepository;

	public EnableDisableApplicationCommandHandler(IUnitOfWork unitOfWork, IAppRepository appRepository)
	{
		_unitOfWork = unitOfWork;
		_appRepository = appRepository;
	}

	public async Task<Result<ApplicationDetailedResponse>> Handle(EnableDisableApplicationCommand request, CancellationToken cancellationToken)
	{
		var appId = new AppId(request.Id);

		var app = await _appRepository.GetByIdAsync(appId);


		if (app is null)
			return Result.Failure<ApplicationDetailedResponse>(AppErrors.NotFound(appId));

		//handle disable/enable
		if (app.IsActive)
		{
			app.Disable();
			app.Raise(new AppDisabledDomainEvent(app.Id));
		}
		else
		{
			app.Enable();
			app.Raise(new AppEnabledDomainEvent(app.Id));
		}

		//Save changes
		_appRepository.Update(app);
		await _unitOfWork.SaveChangesAsync(cancellationToken);

		//Return updated app
		return app.ToApplicationDetailedResponse(); ;
	}

}
