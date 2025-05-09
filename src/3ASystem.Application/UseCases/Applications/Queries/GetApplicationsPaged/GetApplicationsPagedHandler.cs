using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Applications.Responses;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Applications.Queries.GetApplicationsPaged;

public class GetApplicationsPagedHandler : IQueryHandler<GetApplicationsPagedQuery, PagedList<ApplicationCResponse>>
{
	private readonly IAppRepository _appRepository;

	public GetApplicationsPagedHandler(IAppRepository appRepository)
	{
		_appRepository = appRepository;
	}

	public async Task<Result<PagedList<ApplicationCResponse>>> Handle(GetApplicationsPagedQuery request, CancellationToken cancellationToken)
	{

		var skip = (request.Page - 1) * request.PageSize;
		var take = request.PageSize;

		var result = await _appRepository.GetAllAsync(skip, take);

		var applications = result.Records;

		var finalResult = new PagedList<ApplicationCResponse>()
		{
			ActualPage = request.Page,
			TotalOfPages = (int)Math.Ceiling((double)result.TotalOfRecords / request.PageSize),
			TotalOfRecordsPerPage = request.PageSize,
			TotalOfRecords = result.TotalOfRecords,
			

			Records = [.. applications.Select(app =>
				new ApplicationCResponse
				{
					Id = app.Id.Value,
					Name = app.Name,
					Abbreviation = app.Abbreviation,
					IconUrl = app.IconUrl,
					IsActive = app.IsActive
				})]
		};

		return finalResult;
	}

}
