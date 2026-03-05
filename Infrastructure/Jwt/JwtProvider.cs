using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Jwt;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOption _option;
    
    public JwtProvider(IOptions<JwtOption> option)
    {
        _option = option.Value;
    }
    
    public string GenerateToken(User user)
    {
        Claim[] claims =
        [
            new Claim("userId", user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        ];

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_option.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddHours(_option.ExpiresHours));
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}