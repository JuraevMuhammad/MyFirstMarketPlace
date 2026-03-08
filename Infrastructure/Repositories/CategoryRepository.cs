using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IRedisCache _cache;
    private readonly ILogger<CategoryRepository> _logger;
    private const string Key = "category:all";

    public CategoryRepository(ApplicationDbContext context,
        IRedisCache cache,
        ILogger<CategoryRepository> logger)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
    }
    
    public async Task<int> CreateCategoryAsync(Category category)
    {
        var result = _context.Categories
            .FirstOrDefault(x => x.Name == category.Name && !x.IsDeleted);
        if (result != null)
            return 0;
        await _context.Categories.AddAsync(category);
        await _cache.RemoveDataAsync(Key);
        var res = await _context.SaveChangesAsync();
        return res;
    }

    public async Task<List<Category>?> GetCategoriesAsync()
    {
        var cacheCategory = await _cache.GetDataAsync<List<Category>>(Key);
        if (cacheCategory != null)
        {
            _logger.LogInformation("Getting categories from cache");
            return cacheCategory;
        }

        var categories = await _context.Categories
            .Where(x => !x.IsDeleted)
            .ToListAsync();
        
        _logger.LogInformation("Getting categories from database");
        await _cache.SetDataAsync(Key, categories);
        _logger.LogInformation("Save categories to cache");
        return categories;
    }

    public async Task<Category?> GetCategoryAsync(int id)
    {
        var key = $"category:{id}";
        var cacheCategory = await _cache.GetDataAsync<Category>(key);
        if (cacheCategory != null)
        {
            _logger.LogInformation("Getting categories from cache");
            return cacheCategory;
        }
        
        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if(category != null)
            await _cache.SetDataAsync(key, category);
        _logger.LogInformation("Save categories to cache");
        return category;
    }

    public async Task<int> DeleteSaveAsync(int id)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (category == null)
        {
            _logger.LogCritical("Category already exists");
            return 0;
        }

        category.IsDeleted = true;
        
        _logger.LogInformation("Saving category and remove cache");
        await _cache.RemoveDataAsync(Key);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> UpdateSaveAsync(Category category)
    {
        _context.Categories.Update(category);
        var result = await _context.SaveChangesAsync();
        if (result > 0)
            await _cache.RemoveDataAsync(Key);
        return result;
    }
}