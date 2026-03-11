using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Redis;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IRedisCache _cache;
    private const string Key = "order:list";
    private int _cnt = 0;

    public OrderRepository(ApplicationDbContext context, IRedisCache cache)
    {
        _context = context;
        _cache = cache;
    }
    
    public async Task<int> CreateOrder(Order order)
    {
        var product = await _context.Products.Include(x => x.ItemProducts)
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
        if (result > 0)
            await _cache.RemoveDataAsync(Key);
        return result;

    }

    public Task<Order> GetOrder(int orderId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Order>> GetOrders()
    {
        throw new NotImplementedException();
    }
}