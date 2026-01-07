using Microsoft.EntityFrameworkCore;
using SoftwareConsultingPlatform.Core.Models.CaseStudyAggregate;
using SoftwareConsultingPlatform.Core.Models.HomepageAggregate;
using SoftwareConsultingPlatform.Core.Models.ServiceAggregate;
using SoftwareConsultingPlatform.Core.Models.TenantAggregate;
using SoftwareConsultingPlatform.Core.Models.UserAggregate;

namespace SoftwareConsultingPlatform.Core;

/// <summary>
/// Represents the persistence surface for the Software Consulting Platform.
/// This interface provides access to all aggregate roots via DbSet properties.
/// </summary>
public interface ISoftwareConsultingPlatformContext
{
    DbSet<Tenant> Tenants { get; }
    DbSet<User> Users { get; }
    DbSet<Role> Roles { get; }
    DbSet<UserRole> UserRoles { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<UserSession> UserSessions { get; }
    DbSet<ActivityLog> ActivityLogs { get; }
    DbSet<CaseStudy> CaseStudies { get; }
    DbSet<Technology> Technologies { get; }
    DbSet<CaseStudyTechnology> CaseStudyTechnologies { get; }
    DbSet<CaseStudyImage> CaseStudyImages { get; }
    DbSet<Service> Services { get; }
    DbSet<ServiceTechnology> ServiceTechnologies { get; }
    DbSet<ServiceFaq> ServiceFaqs { get; }
    DbSet<ServiceInquiry> ServiceInquiries { get; }
    DbSet<HomepageContent> HomepageContents { get; }
    DbSet<FeaturedItem> FeaturedItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
