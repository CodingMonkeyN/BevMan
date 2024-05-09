namespace BevMan.Domain.Entities;

public class Product : BaseAuditableEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public decimal Quantity { get; set; }
    public Guid? StorageObjectId { get; set; }
    public StorageObject? StorageObject { get; set; }
    public string? PublicUrl { get; set; }
}
