namespace Shared.Core.Interfaces;

public interface ICurrentUser
{
    Guid UserId { get; }
    Guid TenantId { get; }
    string Email { get; }
    IEnumerable<string> Roles { get; }
    bool IsAuthenticated { get; }
}
