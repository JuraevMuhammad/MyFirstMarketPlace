using Domain.Entities;

namespace Infrastructure.Repositories;

public interface IOrderRepository
{
    Task<int?> CreateOrder(Order order);
    Task<Order?> GetOrder(int orderId);
    Task<List<Order>?> GetOrders();
    Task<int> UpdateOrder(Order order);
    Task<List<Order>?> GetMyOrders(int id);
}