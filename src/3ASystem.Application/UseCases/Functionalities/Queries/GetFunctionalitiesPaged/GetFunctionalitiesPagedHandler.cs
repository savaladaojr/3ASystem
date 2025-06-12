using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Applications.Responses;
using _3ASystem.Application.UseCases.Functionalities.Responses;
using _3ASystem.Application.UseCases.Modules.Responses;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Functionalities.Queries.GetFunctionalitiesPaged;

public class GetFunctionalitiesPagedHandler : IQueryHandler<GetFunctionalitiesPagedQuery, PagedList<FunctionalityResponse>>
{
	private readonly IFunctionalityRepository _functionalityRepository;

	public GetFunctionalitiesPagedHandler(IFunctionalityRepository functionalityRepository)
	{
		_functionalityRepository = functionalityRepository;
	}

	public async Task<Result<PagedList<FunctionalityResponse>>> Handle(GetFunctionalitiesPagedQuery request, CancellationToken cancellationToken)
	{

		var skip = (request.Page - 1) * request.PageSize;
		var take = request.PageSize;

		var result = await _functionalityRepository.GetAllAsync(skip, take);

		var functionalities = result.Records;

		var finalResult = new PagedList<FunctionalityResponse>()
		{
			ActualPage = request.Page,
			TotalOfRecordsPerPage = request.PageSize,
			TotalOfRecords = result.TotalOfRecords,

			Records = [.. functionalities.ToIEnumerableOfFunctionalityResponseWithModule()]
		};

		return finalResult;
	}

}
