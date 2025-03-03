using _3ASystem.Domain.Entities;
using _3ASystem.Domain.Entities.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3ASystem.Infrastructure.Data.Configurations
{
	internal class RoleOperationConfiguration : IEntityTypeConfiguration<RoleOperation>
	{
		public void Configure(EntityTypeBuilder<RoleOperation> builder)
		{
			builder.HasKey(ro => ro.RoleId);
			builder.HasKey(ro => ro.OperationId);

			builder.Property(x => x.RoleId)
				.HasConversion(Id => Id.Value, value => new RoleId(value));

			builder.Property(x => x.OperationId)
				.HasConversion(Id => Id.Value, value => new OperationId(value));

			builder.Property(ro => ro.IsAllowed);


			builder.HasOne<Role>()
			    .WithMany(m => m.Operations)
			    .HasForeignKey(ro => ro.RoleId);

			builder.HasOne<Operation>()
				.WithMany()
				.HasForeignKey(ro => ro.OperationId);


			builder.ToTable("RoleOperations");
		}
	}
}
