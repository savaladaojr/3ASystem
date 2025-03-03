using _3ASystem.Domain.Entities;
using _3ASystem.Domain.Entities.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3ASystem.Infrastructure.Data.Configurations
{
	internal class RoleConfiguration : IEntityTypeConfiguration<Role>
	{
		public void Configure(EntityTypeBuilder<Role> builder)
		{
			builder.HasKey(r => r.Id);

			builder.Property(r => r.Id)
				.HasConversion(Id => Id.Value, value => new RoleId(value));

			builder.Property(r => r.ApplicationId)
				.HasConversion(AppId => AppId.Value, value => new AppId(value));

			builder.Property(r => r.Name)
				.HasMaxLength(100);

			builder.Property(r => r.Code)
				.HasMaxLength(25);
			builder.HasIndex(r => r.Code).IsUnique();

			builder.Property(r => r.IsActive);
			builder.HasIndex(r => r.IsActive);


			//EF Relations
			builder.HasOne<App>()
				.WithMany(m => m.Roles)
				.HasForeignKey(f => f.ApplicationId)
				.IsRequired();


			builder.ToTable("Roles");
		}
	}
}
