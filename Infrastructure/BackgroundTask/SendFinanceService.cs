using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.BackgroundTask;

public class SendFinanceService
{
    private readonly IServiceScopeFactory _factory;
    private readonly ISendMail _mail;

    public SendFinanceService(IServiceScopeFactory factory, ISendMail mail)
    {
        _factory = factory;
        _mail = mail;
    }

    public async Task SendMail()
    {
        using var scope = _factory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        var user = context.Users.Where(x => x.Role == Roles.User).ToList();

        foreach (var item in user)
        {
            var userFinance = await context.Finances.FirstOrDefaultAsync(x => x.UserId == item.Id);
            await _mail.SendMailAsync(item, userFinance!);
        }
    }
}