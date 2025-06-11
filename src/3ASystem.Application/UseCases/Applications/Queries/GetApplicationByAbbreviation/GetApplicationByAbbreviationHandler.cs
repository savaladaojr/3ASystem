using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Applications.Responses;
using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Applications.Queries.GetApplicationByAbbreviation;

public class GetApplicationByAbbreviationHandler : IQueryHandler<GetApplicationByAbbreviationQuery, ApplicationDetailedResponse>
{
	private readonly IAppRepository _appRepository;

	public GetApplicationByAbbreviationHandler(IAppRepository appRepository)
	{
		_appRepository = appRepository;
	}

	public async Task<Result<ApplicationDetailedResponse>> Handle(GetApplicationByAbbreviationQuery request, CancellationToken cancellationToken)
	{
		var application = await _appRepository.GetByAbbreviationAsync(request.Abbreviation);

		if (application is null)
			return Result.Failure<ApplicationDetailedResponse>(AppErrors.NotFoundByAbbreviation);

		//return application as ApplicationDetailedResponse
		return application.ToApplicationDetailedResponse();
	}
}
