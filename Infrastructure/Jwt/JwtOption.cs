namespace Infrastructure.Jwt;

public class JwtOption
{
    public string SecretKey { get; set; } = string.Empty;
    public int ExpiresHours { get; set; }
}