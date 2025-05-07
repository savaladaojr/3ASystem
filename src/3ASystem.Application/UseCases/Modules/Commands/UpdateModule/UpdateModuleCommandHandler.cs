using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Applications.Commands.UpdateApplication;
using _3ASystem.Application.UseCases.Applications.Responses;
using _3ASystem.Application.UseCases.Modules.Responses;
using _3ASystem.Domain.Entities.Modules;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Modules.Commands.UpdateModule;

public sealed class UpdateModuleCommandHandler : ICommandHandler<UpdateModuleCommand, ModuleResponse>
{
	private readonly IModuleRepository _moduleRepository;
	private readonly IUnitOfWork _unitOfWork;

	public UpdateModuleCommandHandler(IUnitOfWork unitOfWork, IModuleRepository moduleRepository)
	{
		_moduleRepository = moduleRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<ModuleResponse>> Handle(UpdateModuleCommand request, CancellationToken cancellationToken)
	{
		var moduleId = new ModuleId(request.Id);
		var module = await _moduleRepository.GetByIdAsync(moduleId);

		if (module is null)
			return Result.Failure<ModuleResponse>(ModuleErrors.NotFound(moduleId));

		//Check if the Abbreviation is unique
		var appAbbreviation = await _moduleRepository.GetByAbbreviationAsync(request.Abbreviation);
		if ( appAbbreviation is not null && appAbbreviation.Id != module.Id)
			return Result.Failure<ModuleResponse>(ModuleErrors.AbbreviationNotUnique);

		//Check if the FriendlyID is unique
		var appFriendlyId = await _moduleRepository.GetByFriendlyIdAsync(request.FriendlyId);
		if (appFriendlyId is not null && appFriendlyId.Id != module.Id)
			return Result.Failure<ModuleResponse>(ModuleErrors.FriendlyIdNotUnique);

		module.Update(request.Name, request.Abbreviation, request.Description, request.IconUrl, request.FriendlyId, request.IsPartOfMenu);

		module.Raise(new ModuleUpdatedDomainEvent(module.Id));

		_moduleRepository.Update(module);
		await _unitOfWork.SaveChangesAsync(cancellationToken);

		var finalResult = new ModuleResponse
		{
			Id = module.Id.Value,
			ApplicationId = module.ApplicationId.Value,
			Name = module.Name,
			Abbreviation = module.Abbreviation,
			Description = module.Description,
			IconUrl = module.IconUrl,
			IsActive = module.IsActive,
			FriendlyId = module.FriendlyId,
			IsPartOfMenu = module.IsPartOfMenu,
			CreatedAt = module.CreatedAt,
			LastUpdatedAt = module.LastUpdatedAt
		};

		return finalResult;
	}

}
