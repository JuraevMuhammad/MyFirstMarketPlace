using Application.DTOs.ItemFinance;
using Application.Filter;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Redis;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class FinanceRepository : IFinanceRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IRedisCache _cache;
    private const string Key = "finance:list";    
    public FinanceRepository(ApplicationDbContext context, 
        IRedisCache cache)
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
        var finance = await _context.Finances.FirstOrDefaultAsync(x => x.UserId == id);
        if (finance != null)
            await _cache.SetDataAsync(key, finance);
        return finance;
    }

    public async Task<int> CreateItemFinance(ItemFinance dto, Finance finance)
    {
        await _context.AddAsync(dto);
        
        var res = await _context.SaveChangesAsync();
        if (res <= 0)
            return 0;
        
        _context.Finances.Update(finance);
        var result = await _context.SaveChangesAsync();
        if (result > 0)
            await _cache.RemoveDataAsync(Key);
        return result;
    }

    public async Task<List<ItemFinance>?> GetItemFinances(int id)
    {
        var itemFinance = await _context.ItemFinances.Where(x => x.FinanceId == id).ToListAsync();
        return itemFinance.Count == 0 ? null : itemFinance;
    }

    public async Task<Finance?> GetFinanceFilter(FinanceFilter filter)
    {
        var finance = _context.Finances.AsQueryable();
        
        if(filter.Start != null)
            finance = finance.Where(x => x.CreatedAt)
    }
}