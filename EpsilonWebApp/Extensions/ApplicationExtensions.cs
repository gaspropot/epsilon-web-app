using EpsilonWebApp.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace EpsilonWebApp.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<CustomerApiService>();
        services.AddCascadingAuthenticationState();

        services.AddScoped<AuthService>();
        services.AddScoped<AuthenticationStateProvider>(sp =>
            sp.GetRequiredService<AuthService>());

        return services;
    }
}