﻿using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Domain.Entities.Functionalities;
using Microsoft.EntityFrameworkCore;

namespace _3ASystem.Infrastructure.Data.Repositories;

public sealed class FunctionalityRepository : _Repository<Functionality, FunctionalityId>, IFunctionalityRepository
{
	public FunctionalityRepository(ApplicationDbContext dbContext)
		: base(dbContext)
	{

	}

	public override async Task<Functionality?> GetByIdAsync(FunctionalityId id)
	{
		var module = await Entity.AsNoTracking().Where(item => item.Id == id)
					.Include(m => m.Module) // Include the Module entity
					.ThenInclude(m => m.Application) // Include the Application entity related to the Module
					//.Include(m => m.Operations) // Uncomment if you want to include Operations as well
					.AsSplitQuery() // Use AsSplitQuery to avoid Cartesian product issues with multiple includes			
					.FirstOrDefaultAsync();

		return module;
	}


	public async Task<Functionality?> GetByAbbreviationAsync(string abbreviation)
	{
		var module = await Entity.AsNoTracking().FirstOrDefaultAsync(item => item.Abbreviation == abbreviation);
		return module;
	}

	public async Task<Functionality?> GetByFriendlyIdAsync(string friendlyId)
	{
		var module = await Entity.AsNoTracking().FirstOrDefaultAsync(item => item.FriendlyId == friendlyId);
		return module;
	}

	public async override Task<IPagedResult<Functionality>> GetAllAsync(int skip, int take)
	{
		var count = await Entity.AsNoTracking().CountAsync();

		var records = await Entity.AsNoTracking()
					.OrderBy(ord => ord.CreatedAt)
					.Skip(skip).Take(take)
					.Include(m => m.Module).ThenInclude(m => m.Application)
					.AsSplitQuery() // Use AsSplitQuery to avoid Cartesian product issues with multiple includes
					.ToListAsync();

		var finalResult = new PagedResult<Functionality>
		{
			TotalOfRecords = count,
			Records = records
		};

		return finalResult;
	}
}
