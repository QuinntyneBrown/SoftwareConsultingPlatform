using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoftwareConsultingPlatform.Core.Models.TenantAggregate;

namespace SoftwareConsultingPlatform.Infrastructure.EntityConfigurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.HasKey(t => t.TenantId);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Subdomain)
            .IsRequired()
            .HasMaxLength(63);

        builder.HasIndex(t => t.Subdomain)
            .IsUnique();

        builder.Property(t => t.CustomDomain)
            .HasMaxLength(255);

        builder.HasIndex(t => t.CustomDomain)
            .IsUnique()
            .HasFilter("[CustomDomain] IS NOT NULL");

        builder.Property(t => t.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(t => t.Phone)
            .HasMaxLength(50);

        builder.Property(t => t.Address)
            .HasMaxLength(500);

        builder.Property(t => t.LogoUrl)
            .HasMaxLength(1000);

        builder.Property(t => t.FaviconUrl)
            .HasMaxLength(1000);

        builder.Property(t => t.PrimaryColor)
            .HasMaxLength(7);

        builder.Property(t => t.SecondaryColor)
            .HasMaxLength(7);

        builder.Property(t => t.FontFamily)
            .HasMaxLength(100);

        builder.Property(t => t.FeaturesJson)
            .IsRequired()
            .HasDefaultValue("{}");

        builder.Property(t => t.SettingsJson)
            .IsRequired()
            .HasDefaultValue("{}");

        builder.Property(t => t.MetadataJson)
            .IsRequired()
            .HasDefaultValue("{}");
    }
}
