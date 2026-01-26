using MassTransit;
using Microsoft.EntityFrameworkCore;
using SoftwareConsultingPlatform.Content.Core.Aggregates;

namespace SoftwareConsultingPlatform.Content.Infrastructure.Data;

public class ContentDbContext : DbContext
{
    public ContentDbContext(DbContextOptions<ContentDbContext> options) : base(options) { }
    public DbSet<HomepageContent> HomepageContents => Set<HomepageContent>();
    public DbSet<FeaturedItem> FeaturedItems => Set<FeaturedItem>();

    protected override void OnModelCreating(ModelBuilder m)
    {
        base.OnModelCreating(m);
        m.Entity<HomepageContent>(e => { e.HasKey(x => x.HomepageContentId); e.HasIndex(x => x.TenantId).IsUnique(); });
        m.Entity<FeaturedItem>(e => { e.HasKey(x => x.FeaturedItemId); e.HasIndex(x => new { x.TenantId, x.ItemType, x.ItemId }).IsUnique(); });
        m.AddInboxStateEntity(); m.AddOutboxMessageEntity(); m.AddOutboxStateEntity();
    }
}
