namespace Shared.Messages.Events.Identity;

public record UserLoggedInEvent(
    Guid UserId,
    Guid TenantId,
    string IpAddress,
    string DeviceInfo,
    DateTime LoggedInAt
);
