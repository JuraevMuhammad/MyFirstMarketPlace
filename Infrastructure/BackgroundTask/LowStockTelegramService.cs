using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.BackgroundTask;

public class LowStockTelegramService
{
    private readonly IServiceScopeFactory _factory;
    private readonly ITelegramService _telegram;

    public LowStockTelegramService(IServiceScopeFactory factory,  ITelegramService telegram)
    {
        _factory = factory;
        _telegram = telegram;
    }
    
    public async Task SendMessage()
    {
        using var scope = _factory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var items = await context.ItemProducts
            .Where(x => x.Quantity <= 5)
            .Select(x => new
            {
                Name = x.Product!.Name,
                x.Quantity
            })
            .ToListAsync();

        if (items.Count == 0)
            return;

        var message = "⚠️ Low Stock Products\n\n" +
                      string.Join("\n", items.Select(x => $"• {x.Name} — {x.Quantity} left"));

        await _telegram.SendMessage(message);
    }
}