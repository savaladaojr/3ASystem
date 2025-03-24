﻿using _3ASystem.Domain.Data.Repositories;
using _3ASystem.Domain.Entities.Applications;

namespace _3ASystem.Infrastructure.Data.Repositories;

public sealed class AppRepository : _Repository<App, AppId>, IAppRepository
{
	public AppRepository(ApplicationDbContext dbContext)
		: base(dbContext)
	{

	}

}
