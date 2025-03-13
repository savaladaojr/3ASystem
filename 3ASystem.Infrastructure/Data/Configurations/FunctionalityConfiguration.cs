using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Entities.Functionalities;
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

		builder.Property(f => f.ApplicationId)
			.HasConversion(AppId => AppId.Value, value => new AppId(value));

		builder.Property(f => f.Name)
			.HasMaxLength(100);

		builder.Property(f => f.Route)
			.HasMaxLength(50);

		builder.Property(f => f.Abbreviation)
			.HasMaxLength(25);
		builder.HasIndex(f => f.Abbreviation).IsUnique();

		builder.Property(f => f.IsActive);
		builder.HasIndex(f => f.IsActive);

		//EF Relations
		builder.HasOne<App>()
			.WithMany(m => m.Functionalities)
			.HasForeignKey(f => f.ApplicationId)
			.HasPrincipalKey(a => a.Id)
			.IsRequired();


		builder.Ignore(p => p.Application);
		builder.Ignore(p => p.Operations);

		builder.ToTable("Functionalities");
	}
}
