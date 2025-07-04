﻿using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Applications.Responses;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Applications.Queries.GetApplications;

public class GetApplicationsHandler : IQueryHandler<GetApplicationsQuery, List<ApplicationResponse>>
{
	private readonly IAppRepository _appRepository;

	public GetApplicationsHandler(IAppRepository appRepository)
	{
		_appRepository = appRepository;
	}

	public async Task<Result<List<ApplicationResponse>>> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
	{
		var applications = await _appRepository.GetAllAsync();

		return applications.ToIEnumerableOfApplicationResponse().ToList();
	}

}
