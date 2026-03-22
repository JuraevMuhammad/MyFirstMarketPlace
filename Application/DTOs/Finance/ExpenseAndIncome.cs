namespace Application.DTOs.Finance;

public class ExpenseAndIncome
{
    public string Date { get; set; } = string.Empty;
    public decimal Income { get; set; }
    public decimal Expense { get; set; }
}