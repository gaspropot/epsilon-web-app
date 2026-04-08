using EpsilonWebApp.Components;
using EpsilonWebApp.Extensions;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

//Setup database
builder.Services.AddDatabase(builder.Configuration);

// Setup JWT Authentication
builder.Services.AddJwtCookieAuthentication(builder.Configuration);

// Register services
builder.Services.AddApplicationServices(builder.Configuration);

// For testing purposes, instead of Swagger. Also configuring it for JWT usage
builder.Services.AddOpenApiWithScalar();

// Controllers
builder.Services.AddControllers();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Add MudBlazor
builder.Services.AddMudServices();

var app = builder.Build();

// Seed some data so the grid isn't empty on first run.
await app.SeedDatabaseAsync();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApiWithScalar();
    app.UseWebAssemblyDebugging();
}

// Really important: Authentication → Authorization → Antiforgery → Map endpoints
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
