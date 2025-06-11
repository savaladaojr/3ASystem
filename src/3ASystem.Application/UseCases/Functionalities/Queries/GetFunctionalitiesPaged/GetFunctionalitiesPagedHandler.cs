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

			Records = [.. functionalities.Select(functionality =>
			new FunctionalityResponse
			{
				Id = functionality.Id.Value,
				ModuleId = functionality.ModuleId.Value,
				Name = functionality.Name,
				Abbreviation = functionality.Abbreviation,
				IconUrl = functionality.IconUrl,
				FriendlyId = functionality.FriendlyId,
				IsActive = functionality.IsActive,

				Module = new ModuleResponse
				{
					Id = functionality.Module!.Id.Value,
					Name = functionality.Module.Name,
					Abbreviation = functionality.Module.Abbreviation,
					IconUrl = functionality.Module.IconUrl,
					FriendlyId = functionality.Module.FriendlyId,
					IsActive =  functionality.Module.IsActive,

					Application =  new ApplicationResponse
					{
						Id = functionality.Module.Application!.Id.Value,
						Name = functionality.Module.Application.Name,
						Abbreviation = functionality.Module.Application.Abbreviation,
						IconUrl = functionality.Module.Application.IconUrl,
						FriendlyId = functionality.Module.Application.FriendlyId,
						IsActive = functionality.Module.Application.IsActive
					}
				}
			})]
		};

		return finalResult;
	}

}
