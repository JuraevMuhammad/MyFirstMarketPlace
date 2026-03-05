namespace Infrastructure.Hashing;

public interface IHashPassword
{
    string Generate(string password);
    bool Verify(string password, string hashedPassword);
}