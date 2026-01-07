using SoftwareConsultingPlatform.Core;

namespace SoftwareConsultingPlatform.Infrastructure.Services;

/// <summary>
/// Implementation of ITenantContext that provides the current tenant identifier.
/// </summary>
public class TenantContext : ITenantContext
{
    private Guid _tenantId;

    public Guid TenantId
    {
        get => _tenantId;
        private set => _tenantId = value;
    }

    public void SetTenantId(Guid tenantId)
    {
        _tenantId = tenantId;
    }
}
