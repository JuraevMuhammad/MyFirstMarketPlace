using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.ItemFinance;

public class CreateItemFinance
{
    public decimal Amount { get; set; }
    [Required]
    public required FinanceStatus Status { get; set; }
}