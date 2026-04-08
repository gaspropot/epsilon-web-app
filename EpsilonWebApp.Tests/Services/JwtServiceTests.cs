using EpsilonWebApp.Services;
using Microsoft.Extensions.Configuration;

namespace EpsilonWebApp.Tests.Services;

public class JwtServiceTests
{
    private readonly JwtService _service;

    public JwtServiceTests()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "this-is-a-test-secret-key-that-is-long-enough",
                ["Jwt:Issuer"] = "EpsilonWebApp",
                ["Jwt:Audience"] = "EpsilonWebApp"
            })
            .Build();

        _service = new JwtService(config);
    }

    [Fact]
    public void GenerateToken_ValidCredentials_ReturnsToken()
    {
        var token = _service.GenerateToken("admin", "admin");

        Assert.NotNull(token);
        Assert.NotEmpty(token);
    }

    [Fact]
    public void GenerateToken_ValidCredentials_ReturnsThreePartJwt()
    {
        var token = _service.GenerateToken("admin", "admin");

        Assert.Equal(3, token!.Split('.').Length);
    }

    [Fact]
    public void GenerateToken_WrongPassword_ReturnsNull()
    {
        var token = _service.GenerateToken("admin", "wrongpassword");

        Assert.Null(token);
    }

    [Fact]
    public void GenerateToken_WrongUsername_ReturnsNull()
    {
        var token = _service.GenerateToken("wronguser", "admin");

        Assert.Null(token);
    }

    [Fact]
    public void GenerateToken_WrongCredentials_ReturnsNull()
    {
        var token = _service.GenerateToken("wronguser", "wrongpassword");

        Assert.Null(token);
    }

    [Fact]
    public void GenerateToken_EmptyCredentials_ReturnsNull()
    {
        var token = _service.GenerateToken("", "");

        Assert.Null(token);
    }

    [Fact]
    public void GenerateToken_TwoCallsWithSameCredentials_ReturnsDifferentTokens()
    {
        // Each token should have a unique JTI claim
        var token1 = _service.GenerateToken("admin", "admin");
        var token2 = _service.GenerateToken("admin", "admin");

        Assert.NotEqual(token1, token2);
    }
}