using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Entities.Modules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3ASystem.Infrastructure.Data.Configurations;

public class ModuleConfiguration : IEntityTypeConfiguration<Module>
{
	public void Configure(EntityTypeBuilder<Module> builder)
	{
		builder.HasKey(a => a.Id);

		builder.Property(a => a.Id)
			.HasConversion(Id => Id.Value, value => new ModuleId(value));

		builder.Property(f => f.ApplicationId)
			.HasConversion(AppId => AppId.Value, value => new AppId(value));

		builder.Property(a => a.Name)
			.HasMaxLength(100);

		builder.Property(a => a.Description)
		.HasColumnType("ntext");

		builder.Property(a => a.Abbreviation)
			.HasMaxLength(25);
		builder.HasIndex(a => a.Abbreviation).IsUnique();

		builder.Property(a => a.IsActive);
		builder.HasIndex(a => a.IsActive);

		builder.Property(a => a.IsPartOfMenu);
		builder.HasIndex(a => a.IsPartOfMenu); 

		builder.Property(a => a.FriendlyId)
			.HasMaxLength(25);
		builder.HasIndex(a => a.FriendlyId).IsUnique();

		//EF Relations
		builder.HasOne(m => m.Application)
			.WithMany(m => m.Modules)
			.HasForeignKey(f => f.ApplicationId)
			.HasPrincipalKey(a => a.Id)
			.IsRequired();

		//builder.Ignore(p => p.Application);

		builder.ToTable("Modules");
	}
}
