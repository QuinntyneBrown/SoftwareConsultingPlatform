using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoftwareConsultingPlatform.Core.Models.CaseStudyAggregate;

namespace SoftwareConsultingPlatform.Infrastructure.EntityConfigurations;

public class CaseStudyTechnologyConfiguration : IEntityTypeConfiguration<CaseStudyTechnology>
{
    public void Configure(EntityTypeBuilder<CaseStudyTechnology> builder)
    {
        builder.HasKey(ct => new { ct.CaseStudyId, ct.TechnologyId });

        builder.HasOne(ct => ct.CaseStudy)
            .WithMany()
            .HasForeignKey(ct => ct.CaseStudyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ct => ct.Technology)
            .WithMany()
            .HasForeignKey(ct => ct.TechnologyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
