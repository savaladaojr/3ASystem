using _3ASystem.Domain.Entities.Applications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3ASystem.Infrastructure.Data.Configurations;

public class AppConfiguration : IEntityTypeConfiguration<App>
{
	public void Configure(EntityTypeBuilder<App> builder)
	{
		builder.HasKey(a => a.Id);

		builder.Property(a => a.Id)
			.HasConversion(Id => Id.Value, value => new AppId(value));

		builder.Property(a => a.Name)
			.HasMaxLength(100);

		builder.Property(a => a.Description)
		.HasColumnType("ntext");

		builder.Property(a => a.Abbreviation)
			.HasMaxLength(25);
		builder.HasIndex(a => a.Abbreviation).IsUnique()
			.IsClustered(true);

		builder.Property(a => a.IsActive);
		builder.HasIndex(a => a.IsActive);

		builder.Property(a => a.Hash);
		builder.HasIndex(a => a.Hash);

		builder.Ignore(p => p.Roles);
		builder.Ignore(p => p.Functionalities);

		builder.ToTable("Applications");
	}
}
