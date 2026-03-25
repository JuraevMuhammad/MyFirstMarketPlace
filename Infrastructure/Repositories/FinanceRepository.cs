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
        var finances = await _context.Finances.ToListAsync();
        return finances;
    }

    public async Task<Finance?> GetById(int id)
    {
        var finance = await _context.Finances.FirstOrDefaultAsync(x => x.UserId == id);
        return finance;
    }

    public async Task<int> CreateItemFinance(ItemFinance dto, Finance finance)
    {
        _context.ItemFinances.Add(dto);
        
        var res = await _context.SaveChangesAsync();
        if (res <= 0)
            return 0;
        
        _context.Finances.Update(finance);
        var result = await _context.SaveChangesAsync();
        return result;
    }

    public async Task<List<ItemFinance>?> GetItemFinances(int id)
    {
        var itemFinance = await _context.ItemFinances.Where(x => x.FinanceId == id).ToListAsync();
        return itemFinance.Count == 0 ? null : itemFinance;
    }

    public async Task<List<ItemFinance>?> GetFinanceFilter(FinanceFilter filter)
    {
        var finance = _context.ItemFinances.AsQueryable();

        if (filter.Start != null)
            finance = finance.Where(x => x.CreatedAt >= filter.Start);
        if (filter.Finish != null) 
            finance = finance.Where(x => x.CreatedAt <= filter.Finish);

        return await finance
            .Where(x => !x.IsDeleted)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();
    }

    public async Task<int> CreatedFinance(Finance finance)
    {
        await _context.Finances.AddAsync(finance);
        var result = await _context.SaveChangesAsync();
        return result;
    }
}