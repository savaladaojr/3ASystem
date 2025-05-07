using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Modules.Responses;
using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Entities.Modules;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Modules.Commands.CreateModule;
public class CreateModuleCommandHandler : ICommandHandler<CreateModuleCommand, ModuleResponse>
{
	private readonly IModuleRepository _moduleRepository;
	private readonly IUnitOfWork _unitOfWork;
	
	public CreateModuleCommandHandler(IModuleRepository repository, IUnitOfWork unitOfWork)
	{
		_moduleRepository = repository;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<ModuleResponse>> Handle(CreateModuleCommand request, CancellationToken cancellationToken)
	{
		//Check if the Abbreviation is unique
		var appAbbreviation = await _moduleRepository.GetByAbbreviationAsync(request.Abbreviation);
		if (appAbbreviation is not null)
			return Result.Failure<ModuleResponse>(AppErrors.AbbreviationNotUnique);

		//Check if the FriendlyID is unique
		var appFriendlyId = await _moduleRepository.GetByFriendlyIdAsync(request.FriendlyId);
		if (appFriendlyId is not null)
			return Result.Failure<ModuleResponse>(AppErrors.FriendlyIdNotUnique);

		var appId = new AppId(request.ApplicationId);

		var record = Module.Create(appId, request.Name, request.Abbreviation, request.Description, request.IconUrl, request.FriendlyId, request.IsPartOfMenu);

		var module = _moduleRepository.Create(record);

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
