﻿using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Applications.Responses;
using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Applications.Queries.GetApplicationById;

public class GetApplicationByIdHandler : IQueryHandler<GetApplicationByIdQuery, ApplicationDetailedResponse>
{
	private readonly IAppRepository _appRepository;

	public GetApplicationByIdHandler(IAppRepository appRepository)
	{
		_appRepository = appRepository;
	}

	public async Task<Result<ApplicationDetailedResponse>> Handle(GetApplicationByIdQuery request, CancellationToken cancellationToken)
	{
		var appId = new AppId(request.Id);

		var application = await _appRepository.GetByIdAsync(appId);


		if (application is null)
		{
			return Result.Failure<ApplicationDetailedResponse>(AppErrors.NotFound(appId));
		}

		//return application as ApplicationDetailedResponse
		return application.ToApplicationDetailedResponse();
	}
}
