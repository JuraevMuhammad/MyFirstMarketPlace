using Application.DTOs.Order;
using Application.Responses;
using Domain.Entities;

namespace Application.Interfaces;

public interface IOrderService
{
    Task<Response<string>> CreateOrder(CreatedOrder order);
    Task<Response<List<GetOrder>>> GetOrders();
    Task<Response<GetOrder>> GetOrder(int orderId);
    Task<Response<string>> UpdateOrder(int id, UpdateOrder order);
}