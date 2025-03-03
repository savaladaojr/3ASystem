using _3ASystem.Domain.Entities;
using _3ASystem.Domain.Entities.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3ASystem.Infrastructure.Data.Configurations
{
	internal class OperationConfiguration : IEntityTypeConfiguration<Operation>
	{
		public void Configure(EntityTypeBuilder<Operation> builder)
		{
			builder.HasKey(a => a.Id);
			builder.Property(a => a.Id)
				.HasConversion(Id => Id.Value, value => new OperationId(value));

			builder.Property(a => a.FunctionalityId)
				.HasConversion(Id => Id.Value, value => new FunctionalityId(value));

			builder.Property(a => a.Name)
					.HasMaxLength(100);

			builder.Property(a => a.Code)
					.HasMaxLength(20);
			builder.HasIndex(a => a.Code).IsUnique();

			builder.Property(a => a.IsActive);
			builder.HasIndex(a => a.IsActive);


			builder.HasOne<Functionality>()
				.WithMany(m => m.Operations)
				.HasForeignKey(f => f.FunctionalityId)
				.IsRequired();


			builder.ToTable("Operations");
		}
	}
}
