using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.BackgroundTask;

public class RemoveCategories
{
    private readonly IServiceScopeFactory _factory;

    public RemoveCategories(IServiceScopeFactory factory)
    {
        _factory = factory;
    }

    public async Task RemoveRange()
    {
        using var scope = _factory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var categories = await context.Categories
            .Where(c => !context.Products.Any(p => p.CategoryId == c.Id))
            .ToListAsync();
        
        context.Categories.RemoveRange(categories);
        await context.SaveChangesAsync();
    }
}