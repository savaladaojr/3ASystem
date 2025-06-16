using _3ASystem.Domain.Entities.Functionalities;
using _3ASystem.Domain.Entities.Modules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3ASystem.Infrastructure.Data.Configurations;

public class FunctionalityConfiguration : IEntityTypeConfiguration<Functionality>
{
	public void Configure(EntityTypeBuilder<Functionality> builder)
	{
		builder.HasKey(f => f.Id);
		builder.Property(f => f.Id)
			.HasConversion(Id => Id.Value, value => new FunctionalityId(value));

		builder.Property(f => f.ModuleId)
			.HasConversion(ModuleId => ModuleId.Value, value => new ModuleId(value));

		builder.Property(f => f.Name)
			.HasMaxLength(100);

		builder.Property(f => f.Route)
			.HasMaxLength(50);

		builder.Property(f => f.Abbreviation)
			.HasMaxLength(25);
		builder.HasIndex(f => f.Abbreviation).IsUnique();

		builder.Property(f => f.IsActive);
		builder.HasIndex(f => f.IsActive);

		builder.Property(f => f.IsPartOfMenu);
		builder.HasIndex(f => f.IsPartOfMenu);

		builder.Property(f => f.FriendlyId)
			.HasMaxLength(25);
		builder.HasIndex(f => f.FriendlyId).IsUnique();

		//EF Relations
		builder.HasOne(m => m.Module)
			.WithMany(m => m.Functionalities)
			.HasForeignKey(f => f.ModuleId)
			.HasPrincipalKey(a => a.Id)
			.IsRequired();

		builder.ToTable("Functionalities");
	}
}
