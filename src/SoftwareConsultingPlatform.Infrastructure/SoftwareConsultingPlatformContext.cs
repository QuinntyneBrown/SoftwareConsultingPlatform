using Microsoft.EntityFrameworkCore;
using SoftwareConsultingPlatform.Core;
using SoftwareConsultingPlatform.Core.Models.CaseStudyAggregate;
using SoftwareConsultingPlatform.Core.Models.HomepageAggregate;
using SoftwareConsultingPlatform.Core.Models.ServiceAggregate;
using SoftwareConsultingPlatform.Core.Models.TenantAggregate;
using SoftwareConsultingPlatform.Core.Models.UserAggregate;

namespace SoftwareConsultingPlatform.Infrastructure;

/// <summary>
/// Implementation of the persistence surface for the Software Consulting Platform.
/// </summary>
public class SoftwareConsultingPlatformContext : DbContext, ISoftwareConsultingPlatformContext
{
    private readonly ITenantContext? _tenantContext;

    public SoftwareConsultingPlatformContext(
        DbContextOptions<SoftwareConsultingPlatformContext> options,
        ITenantContext? tenantContext = null)
        : base(options)
    {
        _tenantContext = tenantContext;
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<UserSession> UserSessions => Set<UserSession>();
    public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();
    public DbSet<CaseStudy> CaseStudies => Set<CaseStudy>();
    public DbSet<Technology> Technologies => Set<Technology>();
    public DbSet<CaseStudyTechnology> CaseStudyTechnologies => Set<CaseStudyTechnology>();
    public DbSet<CaseStudyImage> CaseStudyImages => Set<CaseStudyImage>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<ServiceTechnology> ServiceTechnologies => Set<ServiceTechnology>();
    public DbSet<ServiceFaq> ServiceFaqs => Set<ServiceFaq>();
    public DbSet<ServiceInquiry> ServiceInquiries => Set<ServiceInquiry>();
    public DbSet<HomepageContent> HomepageContents => Set<HomepageContent>();
    public DbSet<FeaturedItem> FeaturedItems => Set<FeaturedItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SoftwareConsultingPlatformContext).Assembly);

        // Apply global query filters for multi-tenancy
        if (_tenantContext != null)
        {
            modelBuilder.Entity<User>().HasQueryFilter(e => e.TenantId == _tenantContext.TenantId);
            modelBuilder.Entity<CaseStudy>().HasQueryFilter(e => e.TenantId == _tenantContext.TenantId);
            modelBuilder.Entity<Service>().HasQueryFilter(e => e.TenantId == _tenantContext.TenantId);
            modelBuilder.Entity<HomepageContent>().HasQueryFilter(e => e.TenantId == _tenantContext.TenantId);
            modelBuilder.Entity<FeaturedItem>().HasQueryFilter(e => e.TenantId == _tenantContext.TenantId);
            modelBuilder.Entity<ServiceInquiry>().HasQueryFilter(e => e.TenantId == _tenantContext.TenantId);
            modelBuilder.Entity<RefreshToken>().HasQueryFilter(e => e.TenantId == _tenantContext.TenantId);
            modelBuilder.Entity<UserSession>().HasQueryFilter(e => e.TenantId == _tenantContext.TenantId);
            modelBuilder.Entity<ActivityLog>().HasQueryFilter(e => e.TenantId == _tenantContext.TenantId);
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}
