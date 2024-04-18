namespace BevMan.Domain.Entities;

public class User
{
    public Guid Id { get; init; }
    public required string Email { get; init; }
}
