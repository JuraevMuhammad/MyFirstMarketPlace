using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Redis;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class FinanceRepository : IFinanceRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IRedisCache _cache;
    private const string Key = "finance:list";    
    public FinanceRepository(ApplicationDbContext context, IRedisCache cache)
    {
        _cache = cache;
        _context = context;
    }

    public async Task<List<Finance>?> GetAll()
    {
        var cacheFinances = await _cache.GetDataAsync<List<Finance>>(Key);
        if(cacheFinances != null)
            return cacheFinances;
        var finances = await _context.Finances.ToListAsync();
        if (cacheFinances != null)
            await _cache.SetDataAsync(Key, finances);
        return finances;
    }

    public async Task<Finance?> GetById(int id)
    {
        var key = $"finance:{id}";
        var cacheFinance = await _cache.GetDataAsync<Finance>(key);
        if(cacheFinance != null)
            return cacheFinance;
        var finance = await _context.Finances.FirstOrDefaultAsync(x => x.Id == id);
        if (finance != null)
            await _cache.SetDataAsync(key, finance);
        return finance;
    }
}