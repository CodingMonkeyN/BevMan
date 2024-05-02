namespace BevMan.Domain.Entities;

public class StorageObject
{
    public Guid Id { get; set; }
    public required string BucketId { get; set; }
    public required string[] PathTokens { get; set; }
}
