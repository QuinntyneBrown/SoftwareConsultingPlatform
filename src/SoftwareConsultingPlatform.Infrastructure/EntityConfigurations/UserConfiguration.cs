using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoftwareConsultingPlatform.Core.Models.UserAggregate;

namespace SoftwareConsultingPlatform.Infrastructure.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.UserId);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(u => new { u.TenantId, u.Email })
            .IsUnique()
            .HasFilter("[DeletedAt] IS NULL");

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(u => u.FullName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(u => u.Phone)
            .HasMaxLength(50);

        builder.Property(u => u.CompanyName)
            .HasMaxLength(200);

        builder.Property(u => u.AvatarUrl)
            .HasMaxLength(1000);

        builder.HasIndex(u => u.TenantId);
        builder.HasIndex(u => u.Email);
    }
}
