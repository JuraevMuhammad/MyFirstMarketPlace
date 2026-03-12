using Application.Interfaces;
using Infrastructure.Hashing;
using Infrastructure.Jwt;
using Infrastructure.Redis;
using Infrastructure.Repositories;
using Infrastructure.SaveFile;
using Infrastructure.Services;
using StackExchange.Redis;

namespace WebApp.Extensions;

public static class RegisterServices
{
    public static void AddRegistrationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthorize, AuthorizeService>();
        services.AddScoped<IJwtProvider,  JwtProvider>();
        services.AddScoped<IHashPassword, HashPassword>();
        services.AddScoped<IRedisCache, RedisCache>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IFileStorage, FileStorage>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IItemProductRepository,  ItemProductRepository>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IItemProductService, ItemProductService>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IFinanceRepository, FinanceRepository>();
        services.AddScoped<IFinanceService, FinanceService>();
        services.AddHttpClient<ITelegramService, TelegramService>();
    }
}