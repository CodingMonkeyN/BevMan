namespace BevMan.Domain.Entities;

public class User
{
    public ICollection<UserRole> Roles = new List<UserRole>();
    public Guid Id { get; init; }
    public required string Email { get; init; }
    public Balance Balance { get; set; } = null!;
}
