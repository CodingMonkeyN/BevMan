namespace BevMan.Domain.Entities;

public class BalanceRequest : BaseAuditableEntity
{
    public Guid UserId { get; set; }
    public required User User { get; set; }
    public decimal Amount { get; set; }
}
