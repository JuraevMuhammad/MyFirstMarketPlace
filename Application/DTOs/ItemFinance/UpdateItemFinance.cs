using Domain.Enums;

namespace Application.DTOs.ItemFinance;

public class UpdateItemFinance
{
    public decimal? Amount { get; set; }
    public FinanceStatus? Status { get; set; }
}