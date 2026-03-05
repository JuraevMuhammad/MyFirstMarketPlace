using Domain.Entities;

namespace Infrastructure.Jwt;

public interface IJwtProvider
{
    string GenerateToken(User user);
}