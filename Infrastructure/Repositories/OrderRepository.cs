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

    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    #region CreateOrder

    public async Task<int?> CreateOrder(Order order)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        //quantity - 1
        var product = await _context.Products
            .Include(x => x.ItemProducts)
            .FirstOrDefaultAsync(x => x.Id == order.ProductId);
        if (product == null)
            return 0;
        
        var res = product.ItemProducts.FirstOrDefault(item =>
            item.ColorProduct == order.ColorProduct && item.Size == order.SizeProduct);
        if (res == null || res.Quantity <= 0)
            return null;
        
        res.Quantity -= 1;
        
        order.Sum = product.Price;
        //add order
        await _context.Orders.AddAsync(order);
        var result = await _context.SaveChangesAsync();
        
        if (result <= 0) 
            return result;
        //change finance (newOrder++, totalOrder++)
        var finance = await _context.Finances
            .FirstOrDefaultAsync(x => order.Product != null && x.UserId == order.Product.UserId);
        if (finance == null)
            return 0;
        finance.NewOrders += 1;
        finance.TotalOrders += 1;
        // finance.TotalRevenue += product.Price;
        
        await _context.SaveChangesAsync();
        
        await transaction.CommitAsync();
        
        return result;
    }

    #endregion

    #region GetOrder

    public async Task<Order?> GetOrder(int orderId)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
        return order;
    }

    #endregion

    #region GetOrders

    public async Task<List<Order>?> GetOrders()
    {
        var orders = await _context.Orders.ToListAsync();
        return orders;
    }

    #endregion

    #region UpdateOrder

    public async Task<int> UpdateOrder(Order order)
    {
        var product = await _context.Products.Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == order.ProductId);

        if (product == null)
            return 0;
        
        var finance = await _context.Finances
            .FirstOrDefaultAsync(x => x.UserId == product.UserId);

        switch (order.Status)
        {
            case OrderStatus.Completed when finance != null:
                finance.CompletedOrders += 1;
                finance.TotalRevenue += order.Sum;
                
                finance.NewOrders -= 1;
                break;
            case OrderStatus.Canceled when finance != null:
                finance.CancelledOrders += 1;
                finance.NewOrders -= 1;
                break;
        }
        
        _context.Orders.Update(order);
        var result = await _context.SaveChangesAsync();
        
        return result;
    }

    #endregion

    public async Task<List<Order>?> GetMyOrders(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        
        if(user == null)
            return null;
        
        if(user.Role == Roles.Customer)
            return await _context.Orders
                .Where(x => x.UserId == user.Id)
                .ToListAsync();
        
        if(user.Role == Roles.User)
            return await _context.Orders
                .Where(x => x.Product != null && x.Product.UserId == user.Id)
                .Include(x => x.Product)
                .ToListAsync();
        
        return null;

        // var userCustomer = await _context.Users
        //     .FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id && x.Role == Roles.Customer);
        //
        // if (userCustomer != null)
        // {
        //     var customerOrders = await _context.Orders.Where(x => x.UserId == id).ToListAsync();
        //     return customerOrders;
        // }
        //
        // var userSeller = await _context.Users.Include(x => x.Products)
        //     .FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id && x.Role == Roles.User);
        //
        // if (userSeller == null)
        //     return null;
        //
        // var products = await _context.Products.Where(x => x.UserId == userSeller.Id).ToListAsync();
        //
        // var sellerOrders = await _context.Orders.ToListAsync();
        //
        // List<Order> orders = [];
        //
        // foreach (var item in products)
        // {
        //     var order = sellerOrders.Where(x => x.ProductId == item.Id).ToList();
        //     orders!.AddRange(order);
        // }
        //
        // return orders;
    }
}