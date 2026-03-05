using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Hashing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Seed;

public static class SeedAdmin
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var hash = new HashPassword();
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

        if (await context.Users.AnyAsync(u => u.Role == Roles.Admin))
            return;

        var admin = new User
        {
            Username = "Admin",
            Email = "admin@gmail.com",
            HashPassword = hash.Generate("123456"),
            Role = Roles.Admin,
            CreatedAt = DateTime.UtcNow
        };

        context.Users.Add(admin);
        await context.SaveChangesAsync();
    }
}