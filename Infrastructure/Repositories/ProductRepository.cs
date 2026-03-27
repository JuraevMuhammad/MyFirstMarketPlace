using Application.Filter;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IRedisCache _cache;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ProductRepository> _logger;
    private const string Key = "product:list";

    public ProductRepository(IRedisCache cache, ApplicationDbContext context,
        ILogger<ProductRepository> logger)
    {
        _logger = logger;
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

    public async Task<List<Product>?> GetFilterProducts(ProductFilter filter)
    {
        var products = _context.Products.Include(x => x.ItemProducts).AsQueryable();
        
        if(!string.IsNullOrEmpty(filter.Name))
            products = products.Where(x => x.Name.Contains(filter.Name));
        if(filter.MaxPrice.HasValue)
            products = products.Where(x => x.Price <= filter.MaxPrice.Value);
        if(filter.MinPrice.HasValue)
            products = products.Where(x => x.Price >= filter.MinPrice.Value);
        if(filter.CategoryId.HasValue)
            products = products.Where(x => x.CategoryId == filter.CategoryId.Value);
        
        return await products
            .Where(x => !x.IsDeleted)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize).ToListAsync();
    }

    public async Task<Product?> GetProductById(int id)
    {
        var key = $"product:{id}";
        var cacheProduct = await _cache.GetDataAsync<Product>(key);
        if(cacheProduct != null)
        {
            _logger.LogInformation($"get product from redis cache");
            return cacheProduct;
        }
        _logger.LogInformation($"get product from database");
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id &&  !x.IsDeleted);
        
        if (product == null) 
            return product;
        
        _logger.LogInformation($"Product with id {id} was found and save to redis cache");
        await _cache.SetDataAsync(key, product);
        return product;
    }

    public async Task<List<Product>?> GetMyProducts(int id)
    {
        var products = await _context.Products
            .Include(x => x.ItemProducts)
            .Where(x => x.UserId == id).ToListAsync();
        return products;
    }
}