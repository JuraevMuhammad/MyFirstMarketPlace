using System.Net;
using Application.DTOs.Order;
using Application.Interfaces;
using Application.Responses;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;

    public OrderService(IOrderRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Response<string>> CreateOrder(CreatedOrder order)
    {
        var create = new Order()
        {
            SizeProduct = order.SizeProduct,
            ColorProduct = order.ColorProduct,
            Name = order.Name,
            ProductId = order.ProductId,
            UserId = order.UserId,
            PhoneNumber = order.PhoneNumber,
            Status = OrderStatus.Created,
        };
        var result = await _repository.CreateOrder(create);
        return result > 0
            ? new Response<string>(HttpStatusCode.Created, "created order successfully")
            : new Response<string>(HttpStatusCode.BadRequest, "failed to create order");
    }

    public async Task<Response<List<GetOrder>>> GetOrders()
    {
        var orders = await _repository.GetOrders();
        
        if(orders == null || orders.Count == 0)
            return new Response<List<GetOrder>>(HttpStatusCode.NotFound, "orders not found");

        var getOrders = orders.Select(x => new GetOrder()
        {
            SizeProduct = x.SizeProduct,
            ColorProduct = x.ColorProduct,
            Name = x.Name,
            ProductId = x.ProductId,
            PhoneNumber = x.PhoneNumber,
            UserId = x.UserId,
            Status = x.Status,
            Sum = x.Sum,
            CreateOrder = x.CreatedAt
        }).ToList();
        return new Response<List<GetOrder>>(getOrders);
    }

    public async Task<Response<GetOrder>> GetOrder(int orderId)
    {
        var order = await _repository.GetOrder(orderId);
        
        if(order == null)
            return new Response<GetOrder>(HttpStatusCode.NotFound, "order not found");

        var getOrder = new GetOrder()
        {
            SizeProduct = order.SizeProduct,
            ColorProduct = order.ColorProduct,
            Name = order.Name,
            ProductId = order.ProductId,
            PhoneNumber = order.PhoneNumber,
            UserId = order.UserId,
            Status = order.Status,
            Sum = order.Sum,
            CreateOrder = order.CreatedAt
        };
        return new Response<GetOrder>(getOrder);
    }

    public async Task<Response<string>> UpdateOrder(int id, UpdateOrder order)
    {
        var dbOrder = await _repository.GetOrder(id);
        if (dbOrder == null)
            return new Response<string>(HttpStatusCode.NotFound, "not found");

        dbOrder.Status = order.OrderStatus ?? dbOrder.Status;
        dbOrder.UpdatedAt = DateTime.UtcNow;
        var result = await _repository.UpdateOrder(dbOrder);
        return result > 0
            ? new Response<string>(HttpStatusCode.OK, "updated order successfully")
            : new Response<string>(HttpStatusCode.BadRequest, "updated order failed");
    }
}