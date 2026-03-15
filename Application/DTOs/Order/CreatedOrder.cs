using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.Order;

public class CreatedOrder
{
    public required SizeProduct SizeProduct { get; set; }
    public required ColorProduct ColorProduct { get; set; }
    [Required]
    [Phone]
    public required string PhoneNumber { get; set; }
    [Required]
    public required string Name { get; set; }
    public int ProductId { get; set; }
}