using Application.Interfaces;
using Infrastructure.Hashing;
using Infrastructure.Jwt;
using Infrastructure.Services;

namespace WebApp.Extensions;

public static class RegisterServices
{
    public static void AddRegistrationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthorize, AuthorizeService>();
        services.AddScoped<IJwtProvider,  JwtProvider>();
        services.AddScoped<IHashPassword, HashPassword>();
    }
}