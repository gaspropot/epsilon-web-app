using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

namespace EpsilonWebApp.Extensions;

public static class OpenApiExtensions
{
    public static IServiceCollection AddOpenApiWithScalar(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, ct) =>
            {
                document.Components ??= new();
                document.Components.SecuritySchemes = new Dictionary<string, OpenApiSecurityScheme>
                {
                    ["Bearer"] = new()
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        Description = "Enter your JWT token"
                    }
                };
                return Task.CompletedTask;
            });
        });

        return services;
    }

    public static WebApplication UseOpenApiWithScalar(this WebApplication app)
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            // Just for fun
            options.Title = "Epsilon API";
            options.Theme = ScalarTheme.Purple;
        });

        return app;
    }
}