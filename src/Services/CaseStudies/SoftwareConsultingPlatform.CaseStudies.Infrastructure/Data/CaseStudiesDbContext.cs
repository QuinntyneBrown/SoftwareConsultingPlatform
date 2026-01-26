using MassTransit;
using Microsoft.EntityFrameworkCore;
using SoftwareConsultingPlatform.CaseStudies.Core.Aggregates;

namespace SoftwareConsultingPlatform.CaseStudies.Infrastructure.Data;

public class CaseStudiesDbContext : DbContext
{
    public CaseStudiesDbContext(DbContextOptions<CaseStudiesDbContext> options) : base(options) { }

    public DbSet<CaseStudy> CaseStudies => Set<CaseStudy>();
    public DbSet<Technology> Technologies => Set<Technology>();
    public DbSet<CaseStudyTechnology> CaseStudyTechnologies => Set<CaseStudyTechnology>();
    public DbSet<CaseStudyImage> CaseStudyImages => Set<CaseStudyImage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CaseStudy>(e =>
        {
            e.HasKey(x => x.CaseStudyId);
            e.Property(x => x.ClientName).HasMaxLength(200).IsRequired();
            e.Property(x => x.ProjectTitle).HasMaxLength(200).IsRequired();
            e.Property(x => x.Slug).HasMaxLength(200).IsRequired();
            e.HasIndex(x => new { x.TenantId, x.Slug }).IsUnique();
        });

        modelBuilder.Entity<Technology>(e =>
        {
            e.HasKey(x => x.TechnologyId);
            e.Property(x => x.Name).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<CaseStudyTechnology>(e =>
        {
            e.HasKey(x => new { x.CaseStudyId, x.TechnologyId });
            e.HasOne(x => x.CaseStudy).WithMany(c => c.Technologies).HasForeignKey(x => x.CaseStudyId);
            e.HasOne(x => x.Technology).WithMany(t => t.CaseStudies).HasForeignKey(x => x.TechnologyId);
        });

        modelBuilder.Entity<CaseStudyImage>(e =>
        {
            e.HasKey(x => x.CaseStudyImageId);
            e.HasOne(x => x.CaseStudy).WithMany(c => c.Images).HasForeignKey(x => x.CaseStudyId);
        });

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}
