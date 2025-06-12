using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Modules.Responses;
using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Entities.Modules;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Modules.Queries.GetModuleById;


public class GetModulesByApplicationIdHandler : IQueryHandler<GetModulesByApplicationIdQuery, List<ModuleResponse>>
{
	private readonly IModuleRepository _moduleRepository;

	public GetModulesByApplicationIdHandler(IModuleRepository moduleRepository)
	{
		_moduleRepository = moduleRepository;
	}

	public async Task<Result<List<ModuleResponse>>> Handle(GetModulesByApplicationIdQuery request, CancellationToken cancellationToken)
	{
		var appId = new AppId(request.AppId);

		var modules = await _moduleRepository.GetByApplicationIdAsync(appId);

		return modules.ToIEnumerableOfModuleResponse().ToList();

	}
}
