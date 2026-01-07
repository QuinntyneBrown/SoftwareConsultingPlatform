using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoftwareConsultingPlatform.Core.Models.CaseStudyAggregate;

namespace SoftwareConsultingPlatform.Infrastructure.EntityConfigurations;

public class CaseStudyConfiguration : IEntityTypeConfiguration<CaseStudy>
{
    public void Configure(EntityTypeBuilder<CaseStudy> builder)
    {
        builder.HasKey(c => c.CaseStudyId);

        builder.Property(c => c.ClientName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.ProjectTitle)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(c => c.Slug)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(c => new { c.TenantId, c.Slug })
            .IsUnique();

        builder.Property(c => c.Overview)
            .IsRequired();

        builder.Property(c => c.Challenge)
            .IsRequired();

        builder.Property(c => c.Solution)
            .IsRequired();

        builder.Property(c => c.Results)
            .IsRequired();

        builder.Property(c => c.MetricsJson)
            .IsRequired()
            .HasDefaultValue("[]");

        builder.HasIndex(c => c.TenantId);
        builder.HasIndex(c => c.Status);
        builder.HasIndex(c => c.Featured);
    }
}
