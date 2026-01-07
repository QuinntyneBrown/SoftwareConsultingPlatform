using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoftwareConsultingPlatform.Core.Models.ServiceAggregate;

namespace SoftwareConsultingPlatform.Infrastructure.EntityConfigurations;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.HasKey(s => s.ServiceId);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.Slug)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(s => new { s.TenantId, s.Slug })
            .IsUnique();

        builder.Property(s => s.Tagline)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(s => s.Overview)
            .IsRequired();

        builder.Property(s => s.WhatWeDoJson)
            .IsRequired()
            .HasDefaultValue("[]");

        builder.Property(s => s.HowWeWorkJson)
            .IsRequired()
            .HasDefaultValue("[]");

        builder.Property(s => s.BenefitsJson)
            .IsRequired()
            .HasDefaultValue("[]");

        builder.Property(s => s.PricingModelsJson)
            .IsRequired()
            .HasDefaultValue("[]");

        builder.Property(s => s.EngagementTypesJson)
            .IsRequired()
            .HasDefaultValue("[]");

        builder.HasIndex(s => s.TenantId);
        builder.HasIndex(s => s.Status);
        builder.HasIndex(s => s.DisplayOrder);
    }
}
