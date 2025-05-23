using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Domain.Abstractions;
using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Entities.Functionalities;
using _3ASystem.Domain.Entities.Modules;
using _3ASystem.Domain.Entities.Operations;
using _3ASystem.Domain.Entities.RoleOperations;
using _3ASystem.Domain.Entities.Roles;
using _3ASystem.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace _3ASystem.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext, IUnitOfWork
{
	private readonly IPublisher _publisher;

	public DbSet<App> Applications { get; set; }
	public DbSet<Module> Modules { get; set; }
	public DbSet<Functionality> Functionalities { get; set; }
	public DbSet<Functionality> Functions { get; set; }
	public DbSet<Operation> Operations { get; set; }
	public DbSet<Role> Roles { get; set; }
	public DbSet<RoleOperation> RoleOperations { get; set; }


	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IPublisher publisher) : base(options)
	{
		_publisher = publisher;

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
		// When should you publish domain events?
		//
		// 1. BEFORE calling SaveChangesAsync
		//     - domain events are part of the same transaction
		//     - immediate consistency
		//
		// 2. AFTER calling SaveChangesAsync
		//     - domain events are a separate transaction
		//     - eventual consistency
		//     - handlers can fail

		var result = await base.SaveChangesAsync(cancellationToken);

		await PublishDomainEventsAsync();

		return result;
	}

	private async Task PublishDomainEventsAsync()
	{
		var domainEvents = ChangeTracker
			.Entries<Entity>()
			.Select(entry => entry.Entity)
			.SelectMany(entity =>
			{
				List<IDomainEvent> domainEvents = entity.DomainEvents;

				entity.ClearDomainEvents();

				return domainEvents;
			})
		.ToList();

		foreach (IDomainEvent domainEvent in domainEvents)
		{
			await _publisher.Publish(domainEvent);
		}
	}

}
