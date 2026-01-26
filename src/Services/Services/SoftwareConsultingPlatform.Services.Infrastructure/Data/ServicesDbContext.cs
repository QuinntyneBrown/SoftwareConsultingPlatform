using MassTransit;
using Microsoft.EntityFrameworkCore;
using SoftwareConsultingPlatform.Services.Core.Aggregates;

namespace SoftwareConsultingPlatform.Services.Infrastructure.Data;

public class ServicesDbContext : DbContext
{
    public ServicesDbContext(DbContextOptions<ServicesDbContext> options) : base(options)
    {
    }

    public DbSet<Service> Services => Set<Service>();
    public DbSet<Technology> Technologies => Set<Technology>();
    public DbSet<ServiceTechnology> ServiceTechnologies => Set<ServiceTechnology>();
    public DbSet<ServiceFaq> ServiceFaqs => Set<ServiceFaq>();
    public DbSet<ServiceInquiry> ServiceInquiries => Set<ServiceInquiry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Slug).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Tagline).HasMaxLength(500);
            entity.Property(e => e.IconUrl).HasMaxLength(500);
            entity.Property(e => e.MetaTitle).HasMaxLength(200);
            entity.Property(e => e.MetaDescription).HasMaxLength(500);
            entity.Property(e => e.CanonicalUrl).HasMaxLength(500);

            entity.HasIndex(e => new { e.TenantId, e.Slug }).IsUnique();
            entity.HasIndex(e => e.TenantId);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.DisplayOrder);
        });

        modelBuilder.Entity<Technology>(entity =>
        {
            entity.HasKey(e => e.TechnologyId);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Icon).HasMaxLength(100);
            entity.Property(e => e.Category).HasMaxLength(100);
        });

        modelBuilder.Entity<ServiceTechnology>(entity =>
        {
            entity.HasKey(e => new { e.ServiceId, e.TechnologyId });

            entity.HasOne(e => e.Service)
                .WithMany(s => s.Technologies)
                .HasForeignKey(e => e.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Technology)
                .WithMany(t => t.Services)
                .HasForeignKey(e => e.TechnologyId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ServiceFaq>(entity =>
        {
            entity.HasKey(e => e.ServiceFaqId);
            entity.Property(e => e.Question).HasMaxLength(500).IsRequired();
            entity.Property(e => e.Answer).HasMaxLength(2000).IsRequired();

            entity.HasOne(e => e.Service)
                .WithMany(s => s.Faqs)
                .HasForeignKey(e => e.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ServiceInquiry>(entity =>
        {
            entity.HasKey(e => e.ServiceInquiryId);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(256).IsRequired();
            entity.Property(e => e.Company).HasMaxLength(200);
            entity.Property(e => e.ProjectDescription).HasMaxLength(2000).IsRequired();

            entity.HasIndex(e => new { e.TenantId, e.ServiceId });

            entity.HasOne(e => e.Service)
                .WithMany(s => s.Inquiries)
                .HasForeignKey(e => e.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}
