using Application.DTOs.ItemFinance;

namespace Application.DTOs.Finance;

public class GetFinance
{
    public int Id { get; set; }
    public int CompletedOrders { get; set; }
    public int TotalOrders { get; set; }
    public int NewOrders { get; set; }
    public decimal TotalRevenue { get; set; }
    public int CancelledOrders { get; set; }
    public decimal Expenses { get; set; }
    public decimal Income { get; set; }
    
    public DateTime? Start { get; set; }
    public DateTime? Finish { get; set; }
    
    public List<GetItemFinance>? Items { get; set; }
}