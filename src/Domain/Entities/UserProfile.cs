namespace BevMan.Domain.Entities;

public class UserProfile : BaseAuditableEntity
{
    public required Guid UserId { get; set; }
    public User? User { get; set; }
    public required string DisplayName { get; set; }
    public Guid? StorageObjectId { get; set; }
    public StorageObject? StorageObject { get; set; }
    public string? AvatarUrl { get; set; }
}
