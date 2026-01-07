namespace SoftwareConsultingPlatform.Core.Models.UserAggregate;

/// <summary>
/// Role entity representing a role that can be assigned to users.
/// </summary>
public class Role
{
    public Guid RoleId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Private constructor for EF Core
    private Role() { }

    public Role(string name, string? description = null)
    {
        RoleId = Guid.NewGuid();
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        CreatedAt = DateTime.UtcNow;
    }
}
