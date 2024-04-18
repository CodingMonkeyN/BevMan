namespace BevMan.Domain.Entities;

public class Balance : BaseAuditableEntity
{
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public User? User { get; set; }
}
