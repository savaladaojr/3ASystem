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
		var results = await _functionalityRepository.GetAllAsync();

		return results.ToIEnumerableOfFunctionalityResponse().ToList();

	}

}
