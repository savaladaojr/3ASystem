using _3ASystem.Domain.Entities.Functionalities;
using _3ASystem.Domain.Entities.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3ASystem.Infrastructure.Data.Configurations;

public class OperationConfiguration : IEntityTypeConfiguration<Operation>
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

		builder.Property(f => f.FriendlyId)
			.HasMaxLength(25);
		builder.HasIndex(f => f.FriendlyId).IsUnique();

		builder.HasOne<Functionality>()
			.WithMany(m => m.Operations)
			.HasForeignKey(f => f.FunctionalityId)
			.HasPrincipalKey(p => p.Id)
			.IsRequired();

		builder.ToTable("Operations");
	}
}
