using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Functionalities.Responses;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Functionalities.Queries.GetFunctionalities;

public class GetFunctionalitiesHandler : IQueryHandler<GetFunctionalitiesQuery, List<FunctionalityResponse>>
{
	private readonly IFunctionalityRepository _functionalityRepository;

	public GetFunctionalitiesHandler(IFunctionalityRepository functionalityRepository)
	{
		_functionalityRepository = functionalityRepository;
	}

	public async Task<Result<List<FunctionalityResponse>>> Handle(GetFunctionalitiesQuery request, CancellationToken cancellationToken)
	{

		var result = await _functionalityRepository.GetAllAsync();

		var finalResult = result.Select(functionality =>
			new FunctionalityResponse
			{
				Id = functionality.Id.Value,
				ModuleId = functionality.ModuleId.Value,
				Name = functionality.Name,
				Abbreviation = functionality.Abbreviation,
				IconUrl = functionality.IconUrl,
				FriendlyId = functionality.FriendlyId,
				IsActive = functionality.IsActive

			}
		).ToList();


		return finalResult;
	}

}
