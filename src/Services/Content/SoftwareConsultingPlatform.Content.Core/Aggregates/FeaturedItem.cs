namespace SoftwareConsultingPlatform.Content.Core.Aggregates;

public class FeaturedItem
{
    public Guid FeaturedItemId { get; private set; }
    public Guid TenantId { get; private set; }
    public string ItemType { get; private set; } = string.Empty;
    public Guid ItemId { get; private set; }
    public int DisplayOrder { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private FeaturedItem() { }

    public FeaturedItem(Guid tenantId, string itemType, Guid itemId, int displayOrder)
    {
        FeaturedItemId = Guid.NewGuid();
        TenantId = tenantId;
        ItemType = itemType;
        ItemId = itemId;
        DisplayOrder = displayOrder;
        CreatedAt = DateTime.UtcNow;
    }

    public void SetDisplayOrder(int order) => DisplayOrder = order;
}
