using Domain.Enums;

namespace Application.DTOs.ItemFinance;

public class GetItemFinance
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public FinanceStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}