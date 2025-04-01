using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Domain.Data.Repositories;
using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Shared;
using System.Net.Security;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace _3ASystem.Application.Applications.Queries.GetApplicationById;

public class GetApplicationByIdHandler : IQueryHandler<GetApplicationByIdQuery, ApplicationResponse>
{
	private readonly IAppRepository _appRepository;

	public GetApplicationByIdHandler(IAppRepository appRepository)
	{
		_appRepository = appRepository;
	}

	public async Task<Result<ApplicationResponse>> Handle(GetApplicationByIdQuery request, CancellationToken cancellationToken)
	{
		var appId = new AppId(request.Id);

		var application = await _appRepository.GetByIdAsync(appId);


		if (application is null)
		{
			return Result.Failure<ApplicationResponse>(AppErrors.NotFound(appId));
		}

		var finalResult = new ApplicationResponse() { 
			Abbreviation = application.Abbreviation, 
			Description = application.Description,
			Hash = application.Hash, 
			IconUrl = application.IconUrl,
			Id = application.Id.Value, 
			IsActive = application.IsActive, 
			Name = application.Name 
		};

		return finalResult;
	}
}
