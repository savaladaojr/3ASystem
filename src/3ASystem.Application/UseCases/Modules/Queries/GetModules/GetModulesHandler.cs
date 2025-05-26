using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Applications.Responses;
using _3ASystem.Application.UseCases.Modules.Responses;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Modules.Queries.GetModules;

public class GetModulesHandler : IQueryHandler<GetModulesQuery, List<ModuleResponse>>
{
	private readonly IModuleRepository _moduleRepository;

	public GetModulesHandler(IModuleRepository moduleRepository)
	{
		_moduleRepository = moduleRepository;
	}

	public async Task<Result<List<ModuleResponse>>> Handle(GetModulesQuery request, CancellationToken cancellationToken)
	{
		var modules = await _moduleRepository.GetAllAsync();

		var finalResult = modules.Select(app =>
			new ModuleResponse
			{
				Id = app.Id.Value,
				Name = app.Name,
				Abbreviation = app.Abbreviation,
				IconUrl = app.IconUrl,
				FriendlyId = app.FriendlyId,
				IsActive = app.IsActive
			}
		).ToList();

		return finalResult;
	}

}
