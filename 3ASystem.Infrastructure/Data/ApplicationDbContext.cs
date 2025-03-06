using _3ASystem.Domain.Data;
using _3ASystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace _3ASystem.Infrastructure.Data
{
	public class ApplicationDbContext : DbContext, IApplicationDbContext, IUnitOfWork
	{

		public DbSet<App> Applications { get; set; }
		public DbSet<Functionality> Functionalities { get; set; }
		public DbSet<Functionality> Functions { get; set; }
		public DbSet<Operation> Operations { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<RoleOperation> RoleOperations { get; set; }


		public ApplicationDbContext(DbContextOptions options) : base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//Adding configuration from data configuration classes
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

			foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
			{
				relationship.DeleteBehavior = DeleteBehavior.ClientNoAction;
			}

			//Calling base method
			base.OnModelCreating(modelBuilder);
		}

		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			//var domainEvents = ChangeTracker.Entries<IEntity>()
			//	.Select(s => s.Entity)
			//	.Where(w => w.GetDomainEvents().Any())
			//	.SelectMany(sm =>
			//	{
			//		var domainEvents = sm.GetDomainEvents();
			//		sm.ClearDomainEvents();

			//		return domainEvents;
			//	})
			//	.ToList();

			var result = await base.SaveChangesAsync(cancellationToken);

			//Publish all domain evetns
			//foreach (var domainEvent in domainEvents)
			//{
			//	await _publisher.Publish(domainEvent);
			//}

			return result;
		}

	}
}
