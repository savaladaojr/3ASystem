using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Applications.Responses;
using _3ASystem.Application.UseCases.Modules.Responses;
using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Entities.Modules;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Modules.Commands.EnableDisableModule;

public sealed class EnableDisableModuleCommandHandler : ICommandHandler<EnableDisableModuleCommand, ModuleDetailedResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IModuleRepository _moduleRepository;

	public EnableDisableModuleCommandHandler(IUnitOfWork unitOfWork, IModuleRepository moduleRepository)
	{
		_unitOfWork = unitOfWork;
		_moduleRepository = moduleRepository;
	}

	public async Task<Result<ModuleDetailedResponse>> Handle(EnableDisableModuleCommand request, CancellationToken cancellationToken)
	{
		var moduleId = new ModuleId(request.Id);

		var module = await _moduleRepository.GetByIdAsync(moduleId);


		if (module is null)
			return Result.Failure<ModuleDetailedResponse>(ModuleErrors.NotFound(moduleId));

		//handle disable/enable
		if (module.IsActive)
		{
			module.Disable();
			module.Raise(new ModuleDisabledDomainEvent(module.Id));
		}
		else
		{
			module.Enable();
			module.Raise(new ModuleEnabledDomainEvent(module.Id));
		}

		//Save changes
		_moduleRepository.Update(module);
		await _unitOfWork.SaveChangesAsync(cancellationToken);


		//Return updated module
		var finalResult = new ModuleDetailedResponse
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
