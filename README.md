# Epsilon Web App

A Blazor Server application with full CRUD operations on a Customer entity, built with .NET 9.

## Tech Stack

- **Frontend**: Blazor Server with MudBlazor UI framework
- **Backend**: ASP.NET Core Web API
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: Cookie authentication for the Blazor UI + JWT Bearer for the API

## Projects

- `EpsilonWebApp` — Blazor Server frontend + API controllers
- `EpsilonWebApp.Client` — WASM stub (part of the original template)
- `EpsilonWebApp.Shared` — Shared DTOs referenced by both projects
- `EpsilonWebApp.Tests` — xUnit unit tests

## Prerequisites

- .NET 9 SDK
- SQL Server (local or express)
- EF Core CLI tools: `dotnet tool install --global dotnet-ef`

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/yourusername/epsilon-web-app.git
cd epsilon-web-app
```

### 2. Configure the database

Update the connection string in `EpsilonWebApp/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=EpsilonDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 3. Update the JWT key

Replace the placeholder in `appsettings.json` with a secure key of at least 32 characters:

```json
"Jwt": {
  "Key": "your-super-secret-key-replace-this-with-at-least-32-characters",
  "Issuer": "EpsilonWebApp",
  "Audience": "EpsilonWebApp"
}
```

### 4. Apply migrations

The application will automatically apply migrations and seed the database on first run. Alternatively you can run manually:

```bash
cd EpsilonWebApp
dotnet ef database update
```

### 5. Run the application

```bash
cd EpsilonWebApp
dotnet run
```

Navigate to `https://localhost:7234` in your browser.

## Login Credentials

Username: admin
Password: admin

## Features

- Customer grid with server-side paging
- Create, Edit, Delete customers via MudBlazor UI
- REST API with full CRUD operations
- JWT protected API endpoints (testable via Scalar)
- Cookie authentication for the Blazor UI
- EF Core migrations with automatic database seeder

## API Documentation

Interactive API documentation is available in development mode via Scalar:
https://localhost:7234/scalar/v1

Use the login endpoint to get a JWT token, then authenticate in Scalar to test protected endpoints:
POST /api/auth/login
{
	"username": "admin",
	"password": "admin"
}

## Employee/Manager Design Question

The solution to the Employee/Manager design question is in `EpsilonWebApp/Utilities/NamePrinter.cs`.

The approach uses a shared `INameable` interface implemented by both `Employee` and `Manager`, combined with a generic method constrained to the `INameable` interface.