using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Functionalities.Responses;
using _3ASystem.Application.UseCases.Modules.Queries.GetModuleById;
using _3ASystem.Application.UseCases.Modules.Responses;
using _3ASystem.Domain.Entities.Functionalities;
using _3ASystem.Domain.Entities.Modules;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Functionalities.Queries.GetFunctionalityById;

public class GetFunctionalityByIdHandler : IQueryHandler<GetFunctionalityByIdQuery, FunctionalityDetailedResponse>
{
	private readonly IFunctionalityRepository _functionalityRepository;

	public GetFunctionalityByIdHandler(IFunctionalityRepository functionalityRepository)
	{
		_functionalityRepository = functionalityRepository;
	}

	public async Task<Result<FunctionalityDetailedResponse>> Handle(GetFunctionalityByIdQuery request, CancellationToken cancellationToken)
	{
		var functionalityId = new FunctionalityId(request.Id);

		var functionality = await _functionalityRepository.GetByIdAsync(functionalityId);


		if (functionality is null)
		{
			return Result.Failure<FunctionalityDetailedResponse>(FunctionalityErrors.NotFound(functionalityId));
		}

		var finalResult = new FunctionalityDetailedResponse()
		{
			Id = functionality.Id.Value,
			ModuleId = functionality.ModuleId.Value,
			Name = functionality.Name,
			Abbreviation = functionality.Abbreviation,
			FriendlyId = functionality.FriendlyId,
			IconUrl = functionality.IconUrl,
			IsActive = functionality.IsActive,
			IsPartOfMenu = functionality.IsPartOfMenu,
			CreatedAt = functionality.CreatedAt,
			LastUpdatedAt = functionality.LastUpdatedAt,
		};

		return finalResult;
	}
}
