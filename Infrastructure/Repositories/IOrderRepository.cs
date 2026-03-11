using Domain.Entities;

namespace Infrastructure.Repositories;

public interface IOrderRepository
{
    Task<int> CreateOrder(Order order);
    Task<Order> GetOrder(int orderId);
    Task<List<Order>> GetOrders();
}