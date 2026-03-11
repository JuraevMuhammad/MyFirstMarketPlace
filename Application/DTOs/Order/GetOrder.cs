using Domain.Enums;

namespace Application.DTOs.Order;

public class GetOrder
{
    public SizeProduct SizeProduct { get; set; }
    public ColorProduct ColorProduct { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Sum { get; set; }
    public OrderStatus Status { get; set; }
    
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public DateTime CreateOrder { get; set; }
}