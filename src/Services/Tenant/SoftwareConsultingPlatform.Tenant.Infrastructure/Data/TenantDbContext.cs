using MassTransit;
using Microsoft.EntityFrameworkCore;
using SoftwareConsultingPlatform.Tenant.Core.Aggregates;

namespace SoftwareConsultingPlatform.Tenant.Infrastructure.Data;

public class TenantDbContext : DbContext
{
    public TenantDbContext(DbContextOptions<TenantDbContext> options) : base(options)
    {
    }

    public DbSet<Core.Aggregates.Tenant> Tenants => Set<Core.Aggregates.Tenant>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Core.Aggregates.Tenant>(entity =>
        {
            entity.HasKey(e => e.TenantId);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Subdomain).HasMaxLength(100).IsRequired();
            entity.Property(e => e.CustomDomain).HasMaxLength(200);
            entity.Property(e => e.LogoUrl).HasMaxLength(500);
            entity.Property(e => e.FaviconUrl).HasMaxLength(500);
            entity.Property(e => e.PrimaryColor).HasMaxLength(20);
            entity.Property(e => e.SecondaryColor).HasMaxLength(20);
            entity.Property(e => e.FontFamily).HasMaxLength(100);
            entity.Property(e => e.ContactEmail).HasMaxLength(256);
            entity.Property(e => e.ContactPhone).HasMaxLength(50);
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.SupportEmail).HasMaxLength(256);
            entity.Property(e => e.Plan).HasMaxLength(100);
            entity.Property(e => e.BillingEmail).HasMaxLength(256);
            entity.Property(e => e.SubscriptionStatus).HasMaxLength(50);

            entity.HasIndex(e => e.Subdomain).IsUnique();
            entity.HasIndex(e => e.CustomDomain);
            entity.HasIndex(e => e.Status);
        });

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}
