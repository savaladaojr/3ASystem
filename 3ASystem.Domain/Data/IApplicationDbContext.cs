using _3ASystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace _3ASystem.Domain.Data
{
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

}
