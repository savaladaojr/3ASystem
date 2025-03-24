using _3ASystem.Domain.Entities.Operations;
using _3ASystem.Domain.Entities.RoleOperations;
using _3ASystem.Domain.Entities.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3ASystem.Infrastructure.Data.Configurations;

public class RoleOperationConfiguration : IEntityTypeConfiguration<RoleOperation>
{
	public void Configure(EntityTypeBuilder<RoleOperation> builder)
	{
		builder.HasKey(k => new { k.RoleId, k.OperationId });

		builder.Property(x => x.RoleId)
			.HasConversion(Id => Id.Value, value => new RoleId(value));

		builder.Property(x => x.OperationId)
			.HasConversion(Id => Id.Value, value => new OperationId(value));

		builder.Property(ro => ro.IsAllowed);


		builder.HasOne<Role>()
		    .WithMany()
		    .HasForeignKey(ro => ro.RoleId)
			.HasPrincipalKey(p => p.Id);

		builder.HasOne<Operation>()
			.WithMany()
			.HasForeignKey(ro => ro.OperationId)
			.HasPrincipalKey(p => p.Id);

		builder.Ignore(p => p.Role);
		builder.Ignore(p => p.Operation);


		builder.ToTable("RoleOperations");
	}
}
