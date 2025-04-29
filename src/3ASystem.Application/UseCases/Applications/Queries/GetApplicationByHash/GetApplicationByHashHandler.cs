using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Applications.Responses;
using _3ASystem.Domain.Data.Repositories;
using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Applications.Queries.GetApplicationByHash;

public class GetApplicationByHashHandler : IQueryHandler<GetApplicationByHashQuery, ApplicationResponse>
{
	private readonly IAppRepository _appRepository;

	public GetApplicationByHashHandler(IAppRepository appRepository)
	{
		_appRepository = appRepository;
	}

	public async Task<Result<ApplicationResponse>> Handle(GetApplicationByHashQuery request, CancellationToken cancellationToken)
	{
		var application = await _appRepository.GetByHashAsync(request.Hash);

		if (application is null)
			return Result.Failure<ApplicationResponse>(AppErrors.NotFoundByHash);


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
			UpdatedAt = application.LastUpdatedAt
		};

		return finalResult;
	}
}
