using Microsoft.EntityFrameworkCore;

namespace BevMan.Domain.Entities;

[Index(nameof(UserId), nameof(Role), IsUnique = true)]
public class UserRole : BaseAuditableEntity
{
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public AppRole Role { get; set; }
}
