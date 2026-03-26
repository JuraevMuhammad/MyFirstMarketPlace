using System.Net;
using System.Security.Claims;
using Application.DTOs.Order;
using Application.Interfaces;
using Application.Responses;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Repositories;
using Infrastructure.Telegram;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly IHttpContextAccessor _accessor;
    private readonly IOrderRepository _repository;
    private readonly IUserRepository _userRepository;
    private readonly ITelegramService _telegram;

    public OrderService(IOrderRepository repository,
        ITelegramService telegram,
        IHttpContextAccessor accessor,
        IUserRepository userRepository)
    {
        _telegram = telegram;
        _userRepository = userRepository;
        _repository = repository;
        _accessor = accessor;
    }
    
    public async Task<Response<string>> CreateOrder(CreatedOrder order)
    {
        var id = _accessor.HttpContext?.User.FindFirstValue("userId");
        
        if(!int.TryParse(id, out var userId))
            return new Response<string>(HttpStatusCode.Unauthorized, "invalid token");

        var user = await _userRepository.GetUserByIdAsync(userId);
        
        var create = new Order()
        {
            SizeProduct = order.SizeProduct,
            ColorProduct = order.ColorProduct,
            Name = user!.Username,
            ProductId = order.ProductId,
            UserId = userId,
            PhoneNumber = order.PhoneNumber,
            Status = OrderStatus.Created,
        };
        var result = await _repository.CreateOrder(create);
        if(result <= 0)
            return new Response<string>(HttpStatusCode.BadRequest, "failed to create order");

        var telegramMessage = $"""
                               пришёл заказ №{create.Id}
                               Customer Name: {create.Name}
                               Customer Phone Number: {create.PhoneNumber}
                               
                               ProductId: {create.ProductId}
                               ColorProduct: {create.ColorProduct}
                               SizeProduct: {create.SizeProduct}
                               
                               Price: {create.Sum}
                               """;
        
        await _telegram.SendMessage(telegramMessage);
        return new Response<string>(HttpStatusCode.Created, "created order successfully");
    }

    public async Task<Response<List<GetOrder>>> GetOrders()
    {
        var orders = await _repository.GetOrders();
        
        if(orders == null || orders.Count == 0)
            return new Response<List<GetOrder>>(HttpStatusCode.NotFound, "orders not found");

        var getOrders = orders.Select(x => new GetOrder()
        {
            Id = x.Id,
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
            Id = order.Id,
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