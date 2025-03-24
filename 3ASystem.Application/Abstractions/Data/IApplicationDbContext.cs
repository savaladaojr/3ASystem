using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Entities.Functionalities;
using _3ASystem.Domain.Entities.Operations;
using _3ASystem.Domain.Entities.RoleOperations;
using _3ASystem.Domain.Entities.Roles;
using Microsoft.EntityFrameworkCore;

namespace _3ASystem.Application.Abstractions.Data;

public interface IApplicationDbContext
{
	//#region DBSets
	DbSet<App> Applications { get; set; }
	DbSet<Functionality> Functions { get; set; }
	DbSet<Operation> Operations { get; set; }
	DbSet<Role> Roles { get; set; }
	DbSet<RoleOperation> RoleOperations { get; set; }
	//#endregion

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
