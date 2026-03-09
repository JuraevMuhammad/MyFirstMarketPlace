using Application.Filter;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Redis;

namespace Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IRedisCache _cache;
    private readonly ApplicationDbContext _context;
    private const string Key = "product:list";

    public ProductRepository(IRedisCache cache, ApplicationDbContext context)
    {
        _cache = cache;
        _context = context;
    }
    
    public async Task<int> CreateProduct(Product product)
    {
        await _context.Products.AddAsync(product);
        var result = await _context.SaveChangesAsync();
        if (result > 0)
            await _cache.RemoveDataAsync(Key);
        return result;
    }

    public async Task<int> UpdateProduct(Product product)
    {
        _context.Products.Update(product);
        var result = await _context.SaveChangesAsync();
        if (result > 0)
            await _cache.RemoveDataAsync(Key);
        return result;
    }

    public async Task<List<Product>> GetFilterProducts(ProductFilter filter)
    {
        throw new NotImplementedException();
    }

    public async Task<Product> GetProductById(int id)
    {
        throw new NotImplementedException();
    }
}