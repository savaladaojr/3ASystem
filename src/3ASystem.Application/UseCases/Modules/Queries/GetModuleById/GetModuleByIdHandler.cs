using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Modules.Responses;
using _3ASystem.Domain.Data.Repositories;
using _3ASystem.Domain.Entities.Modules;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Modules.Queries.GetModuleById;

public class GetModuleByIdHandler : IQueryHandler<GetModuleByIdQuery, ModuleResponse>
{
	private readonly IModuleRepository _moduleRepository;

	public GetModuleByIdHandler(IModuleRepository moduleRepository)
	{
		_moduleRepository = moduleRepository;
	}

	public async Task<Result<ModuleResponse>> Handle(GetModuleByIdQuery request, CancellationToken cancellationToken)
	{
		var moduleId = new ModuleId(request.Id);

		var module = await _moduleRepository.GetByIdAsync(moduleId);


		if (module is null)
		{
			return Result.Failure<ModuleResponse>(ModuleErrors.NotFound(moduleId));
		}

		var finalResult = new ModuleResponse()
		{
			Id = module.Id.Value,
			ApplicationId = module.ApplicationId.Value,
			Name = module.Name,
			Description = module.Description,
			CreatedAt = module.CreatedAt,
			UpdatedAt = module.LastUpdatedAt,
		};

		return finalResult;
	}
}
