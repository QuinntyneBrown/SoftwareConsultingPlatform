namespace SoftwareConsultingPlatform.Core;

/// <summary>
/// Provides access to the current tenant context.
/// </summary>
public interface ITenantContext
{
    /// <summary>
    /// Gets the current tenant identifier.
    /// </summary>
    Guid TenantId { get; }
}
