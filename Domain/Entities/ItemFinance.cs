using Domain.Enums;

namespace Domain.Entities;

public class ItemFinance : BaseEntity
{
    public decimal Amount { get; set; }
    public FinanceStatus Status { get; set; } = FinanceStatus.Expense;
    
    public int FinanceId { get; set; }
    public Finance? Finance { get; set; }
}
