using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.Applications.Shared;
using _3ASystem.Domain.Data.Repositories;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.Applications.Queries.GetApplications;

public class GetApplicationsHandler : IQueryHandler<GetApplicationsQuery, List<ApplicationCResponse>>
{
	private readonly IAppRepository _appRepository;

	public GetApplicationsHandler(IAppRepository appRepository)
	{
		_appRepository = appRepository;
	}

	public async Task<Result<List<ApplicationCResponse>>> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
	{
		var applications = await _appRepository.GetAllAsync();

		var finalResult = applications.Select(app =>
			new ApplicationCResponse
			{
				Id = app.Id.Value,
				Name = app.Name,
				Abbreviation = app.Abbreviation,
				IconUrl = app.IconUrl,
				IsActive = app.IsActive
			}
		).ToList();

		return finalResult;
	}

}
