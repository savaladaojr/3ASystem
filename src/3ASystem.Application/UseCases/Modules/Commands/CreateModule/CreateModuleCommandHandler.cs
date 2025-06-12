using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Functionalities.Responses;
using _3ASystem.Application.UseCases.Modules.Responses;
using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Entities.Modules;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Modules.Commands.CreateModule;
public class CreateModuleCommandHandler : ICommandHandler<CreateModuleCommand, ModuleDetailedResponse>
{
	private readonly IModuleRepository _moduleRepository;
	private readonly IAppRepository _appRepository;
	private readonly IUnitOfWork _unitOfWork;
	
	public CreateModuleCommandHandler(IModuleRepository repository, IAppRepository appRepository, IUnitOfWork unitOfWork)
	{
		_moduleRepository = repository;
		_appRepository = appRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<ModuleDetailedResponse>> Handle(CreateModuleCommand request, CancellationToken cancellationToken)
	{
		//Check if the Abbreviation is unique
		var appAbbreviation = await _moduleRepository.GetByAbbreviationAsync(request.Abbreviation);
		if (appAbbreviation is not null)
			return Result.Failure<ModuleDetailedResponse>(ModuleErrors.AbbreviationNotUnique);

		//Check if the FriendlyID is unique
		var appFriendlyId = await _moduleRepository.GetByFriendlyIdAsync(request.FriendlyId);
		if (appFriendlyId is not null)
			return Result.Failure<ModuleDetailedResponse>(ModuleErrors.FriendlyIdNotUnique);

		//Check if the Application exists
		var applicationId = new AppId(request.ApplicationId);
		var application = await _appRepository.GetByIdAsync(applicationId);
		if (application is null)
			return Result.Failure<ModuleDetailedResponse>(AppErrors.NotFound(applicationId));

		var record = Module.Create(applicationId, request.Name, request.Abbreviation, request.Description, request.IconUrl, request.FriendlyId, request.IsPartOfMenu);

		record.Raise(new ModuleCreatedDomainEvent(record.Id));

		var module = _moduleRepository.Create(record);

		await _unitOfWork.SaveChangesAsync(cancellationToken);

		// Return created module
		return module.ToModuleDetailedResponse();
	}
}
