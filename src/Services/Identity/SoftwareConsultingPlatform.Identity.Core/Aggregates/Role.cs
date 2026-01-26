namespace SoftwareConsultingPlatform.Identity.Core.Aggregates;

public class Role
{
    public Guid RoleId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();

    private Role() { }

    public Role(string name, string? description = null)
    {
        RoleId = Guid.NewGuid();
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        CreatedAt = DateTime.UtcNow;
    }
}
