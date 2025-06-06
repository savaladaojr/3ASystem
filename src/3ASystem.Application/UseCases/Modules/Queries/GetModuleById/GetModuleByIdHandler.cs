using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Modules.Responses;
using _3ASystem.Domain.Entities.Modules;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Modules.Queries.GetModuleById;

public class GetModuleByIdHandler : IQueryHandler<GetModuleByIdQuery, ModuleDetailedResponse>
{
	private readonly IModuleRepository _moduleRepository;

	public GetModuleByIdHandler(IModuleRepository moduleRepository)
	{
		_moduleRepository = moduleRepository;
	}

	public async Task<Result<ModuleDetailedResponse>> Handle(GetModuleByIdQuery request, CancellationToken cancellationToken)
	{
		var moduleId = new ModuleId(request.Id);

		var module = await _moduleRepository.GetByIdAsync(moduleId);


		if (module is null)
		{
			return Result.Failure<ModuleDetailedResponse>(ModuleErrors.NotFound(moduleId));
		}

		var finalResult = new ModuleDetailedResponse()
		{
			Id = module.Id.Value,
			ApplicationId = module.ApplicationId.Value,
			Name = module.Name,
			Abbreviation = module.Abbreviation,
			Description = module.Description,
			FriendlyId = module.FriendlyId,
			IconUrl = module.IconUrl,
			IsActive = module.IsActive,
			IsPartOfMenu = module.IsPartOfMenu,
			CreatedAt = module.CreatedAt,
			LastUpdatedAt = module.LastUpdatedAt,
		};

		return finalResult;
	}
}
