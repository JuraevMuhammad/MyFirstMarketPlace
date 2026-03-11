using Application.DTOs.Order;
using Application.Responses;
using Domain.Entities;

namespace Application.Interfaces;

public interface IOrderService
{
    Task<Response<string>> CreateOrder(CreatedOrder order);
}