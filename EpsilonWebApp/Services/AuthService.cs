using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace EpsilonWebApp.Services;

public class AuthService : AuthenticationStateProvider
{
    private readonly IJwtService _jwtService;
    private readonly ProtectedSessionStorage _sessionStorage;
    private string? _token;

    private static readonly AuthenticationState Anonymous =
        new(new ClaimsPrincipal(new ClaimsIdentity()));

    public AuthService(IJwtService jwtService, ProtectedSessionStorage sessionStorage)
    {
        _jwtService = jwtService;
        _sessionStorage = sessionStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (!string.IsNullOrEmpty(_token))
        {
            var identity = new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.Name, "admin")
        }, "jwt");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        try
        {
            var result = await _sessionStorage.GetAsync<string>("jwt_token");
            if (result.Success && !string.IsNullOrEmpty(result.Value))
            {
                _token = result.Value;
                var identity = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, "admin")
            }, "jwt");
                return new AuthenticationState(new ClaimsPrincipal(identity));
            }
        }
        catch
        {
            // JS interop not available during prerender - return anonymous
            return Anonymous;
        }

        return Anonymous;
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        var token = _jwtService.GenerateToken(username, password);
        if (token is null)
            return false;

        _token = token;
        await _sessionStorage.SetAsync("jwt_token", token);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        return true;
    }

    public async Task LogoutAsync()
    {
        _token = null;
        await _sessionStorage.DeleteAsync("jwt_token");
        NotifyAuthenticationStateChanged(Task.FromResult(Anonymous));
    }

    public string? GetToken() => _token;
}