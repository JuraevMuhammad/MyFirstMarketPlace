using Domain.Enums;

namespace Domain.Entities;

public class Finance : BaseEntity
{
    public int CompletedOrders { get; set; }
    public int TotalOrders { get; set; }
    public int NewOrders { get; set; }
    public decimal TotalRevenue { get; set; }
    public int CancelledOrders { get; set; }
    public decimal Expenses { get; set; }
    public decimal Income { get; set; }
    
    public List<ItemFinance>? Items { get; set; }
    public User? User { get; set; }
    public int UserId { get; set; }
}