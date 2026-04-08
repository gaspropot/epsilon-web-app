namespace EpsilonWebApp.Services;

public interface IJwtService
{
    string? GenerateToken(string username, string password);
}