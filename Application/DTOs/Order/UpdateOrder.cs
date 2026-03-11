using Domain.Enums;

namespace Application.DTOs.Order;

public class UpdateOrder
{
    public OrderStatus? OrderStatus { get; set; }
}