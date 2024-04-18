using Microsoft.EntityFrameworkCore;

namespace BevMan.Domain.Entities;

[Index(nameof(UserId), nameof(Role), IsUnique = true)]
public class UserRole
{
    public long Id { get; set; }
    public Guid UserId { get; set; }
    public AppRole Role { get; set; }
}
