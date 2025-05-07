using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Applications.Responses;
using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Applications.Queries.GetApplicationByAbbreviation;

public class GetApplicationByAbbreviationHandler : IQueryHandler<GetApplicationByAbbreviationQuery, ApplicationResponse>
{
	private readonly IAppRepository _appRepository;

	public GetApplicationByAbbreviationHandler(IAppRepository appRepository)
	{
		_appRepository = appRepository;
	}

	public async Task<Result<ApplicationResponse>> Handle(GetApplicationByAbbreviationQuery request, CancellationToken cancellationToken)
	{
		var application = await _appRepository.GetByAbbreviationAsync(request.Abbreviation);

		if (application is null)
			return Result.Failure<ApplicationResponse>(AppErrors.NotFoundByAbbreviation);


		var finalResult = new ApplicationResponse() { 
			Abbreviation = application.Abbreviation, 
			Description = application.Description,
			Hash = application.Hash, 
			IconUrl = application.IconUrl,
			Id = application.Id.Value, 
			IsActive = application.IsActive, 
			Name = application.Name,
			FriendlyId = application.FriendlyId,
			CreatedAt = application.CreatedAt,
			LastUpdatedAt = application.LastUpdatedAt
		};

		return finalResult;
	}
}
