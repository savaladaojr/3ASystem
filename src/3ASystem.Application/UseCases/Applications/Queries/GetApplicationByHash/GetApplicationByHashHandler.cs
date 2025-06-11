using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Applications.Responses;
using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Applications.Queries.GetApplicationByHash;

public class GetApplicationByHashHandler : IQueryHandler<GetApplicationByHashQuery, ApplicationDetailedResponse>
{
	private readonly IAppRepository _appRepository;

	public GetApplicationByHashHandler(IAppRepository appRepository)
	{
		_appRepository = appRepository;
	}

	public async Task<Result<ApplicationDetailedResponse>> Handle(GetApplicationByHashQuery request, CancellationToken cancellationToken)
	{
		var application = await _appRepository.GetByHashAsync(request.Hash);

		if (application is null)
			return Result.Failure<ApplicationDetailedResponse>(AppErrors.NotFoundByHash);

		//return application as ApplicationDetailedResponse
		return application.ToApplicationDetailedResponse();
	}
}
