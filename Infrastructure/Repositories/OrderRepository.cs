using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IRedisCache _cache;
    private const string Key = "order:list";

    public OrderRepository(ApplicationDbContext context, IRedisCache cache)
    {
        _context = context;
        _cache = cache;
    }
    
    public async Task<int> CreateOrder(Order order)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        
        var product = await _context.Products
            .Include(x => x.ItemProducts)
            .FirstOrDefaultAsync(x => x.Id == order.ProductId);
        if (product == null)
            return 0;
        
        var customer = await _context.Users
            .AnyAsync(x => x.Id == order.UserId);
        if (!customer)
            return 0;
        
        var res = product.ItemProducts?.FirstOrDefault(item =>
            item.ColorProduct == order.ColorProduct && item.Size == order.SizeProduct);
        if (res == null || res.Quantity <= 0)
            return 0;
        
        res.Quantity -= 1;
        
        order.Sum = product.Price;
        
        await _context.Orders.AddAsync(order);
        var result = await _context.SaveChangesAsync();
        
        if (result <= 0) 
            return result;
        
        var finance = await _context.Finances
            .FirstOrDefaultAsync(x => x.UserId == order.UserId);
        if (finance == null)
            return 0;
        finance.NewOrders += 1;
        finance.TotalOrders += 1;
        finance.TotalRevenue += product.Price;

        await _cache.RemoveDataAsync($"finance:{finance.UserId}");
        
        await _context.SaveChangesAsync();
        
        await transaction.CommitAsync();
        
        await _cache.RemoveDataAsync(Key);
        
        return result;
    }

    public async Task<Order?> GetOrder(int orderId)
    {
        var key = $"order:{orderId}";
        var cacheOrder = await _cache.GetDataAsync<Order>(key);
        if (cacheOrder != null)
            return cacheOrder;
        var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
        await _cache.SetDataAsync(key, order);
        return order;
    }

    public async Task<List<Order>?> GetOrders()
    {
        var cacheOrders = await _cache.GetDataAsync<List<Order>?>(Key);
        if (cacheOrders != null)
            return cacheOrders;
        var orders = await _context.Orders.ToListAsync();
        await _cache.SetDataAsync(Key, orders);
        return orders;
    }

    public async Task<int> UpdateOrder(Order order)
    {
        var finance = await _context.Finances
            .FirstOrDefaultAsync(x => x.UserId == order.UserId);

        switch (order.Status)
        {
            case OrderStatus.Completed when finance != null:
                finance.CompletedOrders += 1;
                finance.NewOrders -= 1;
                break;
            case OrderStatus.Canceled when finance != null:
                finance.CancelledOrders += 1;
                finance.NewOrders -= 1;
                break;
        }

        _context.Orders.Update(order);
        var result = await _context.SaveChangesAsync();
        if (result > 0)
            await _cache.RemoveDataAsync(Key);
        return result;
    }
}