﻿using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Applications.Responses;
using _3ASystem.Application.UseCases.Modules.Responses;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Modules.Queries.GetModulesPaged;

public class GetModulesPagedHandler : IQueryHandler<GetModulesPagedQuery, PagedList<ModuleResponse>>
{
	private readonly IModuleRepository _moduleRepository;

	public GetModulesPagedHandler(IModuleRepository moduleRepository)
	{
		_moduleRepository = moduleRepository;
	}

	public async Task<Result<PagedList<ModuleResponse>>> Handle(GetModulesPagedQuery request, CancellationToken cancellationToken)
	{

		var skip = (request.Page - 1) * request.PageSize;
		var take = request.PageSize;

		var result = await _moduleRepository.GetAllAsync(skip, take);

		var modules = result.Records;

		var finalResult = new PagedList<ModuleResponse>()
		{
			ActualPage = request.Page,
			TotalOfRecordsPerPage = request.PageSize,
			TotalOfRecords = result.TotalOfRecords,

			Records = [.. modules.ToIEnumerableOfModuleResponseWithApplication()]
		};

		return finalResult;
	}

}
