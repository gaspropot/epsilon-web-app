using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EpsilonWebApp.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    // Obviously this is just for demonstration - in a real app, you'd check against a user store
    private const string _adminUsername = "admin";
    private const string _adminPassword = "admin";

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string? GenerateToken(string username, string password)
    {
        if (username != _adminUsername || password != _adminPassword)
            return null;

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}