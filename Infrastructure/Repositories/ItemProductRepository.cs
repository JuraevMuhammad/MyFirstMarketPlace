using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Redis;

namespace Infrastructure.Repositories;

public class ItemProductRepository : IItemProductRepository
{
    private readonly IRedisCache _cache;
    private readonly ApplicationDbContext _context;
    private const string Key = "itemproduct:list";

    public ItemProductRepository(IRedisCache cache, ApplicationDbContext context)
    {
        _cache = cache;
        _context = context;
    }
    
    public async Task<int> CreateItemProduct(ItemProduct product)
    {
        await _context.ItemProducts.AddAsync(product);
        var result = await _context.SaveChangesAsync();
        if (result > 0)
            await _cache.RemoveDataAsync(Key);
        return result;
    }
}