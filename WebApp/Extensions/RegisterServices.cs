using Application.Interfaces;
using Infrastructure.Hashing;
using Infrastructure.Jwt;
using Infrastructure.Redis;
using Infrastructure.Repositories;
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
    }
}