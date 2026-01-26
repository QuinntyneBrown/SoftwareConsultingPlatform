namespace Shared.Core.Interfaces;

public interface ITenantContext
{
    Guid TenantId { get; }
}
