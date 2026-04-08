using EpsilonWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EpsilonWebApp.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await context.Database.MigrateAsync();

        // Only seed if the table is empty
        if (await context.Customers.AnyAsync())
            return;

        var customers = new List<Customer>
        {
            new() {
                CompanyName = "Acme Corp",
                ContactName = "John Smith",
                Address = "123 Main St",
                City = "New York",
                Region = "NY",
                PostalCode = "10001",
                Country = "USA",
                Phone = "555-0100"
            },
            new() {
                CompanyName = "Globex Corporation",
                ContactName = "Jane Doe",
                Address = "456 Industrial Ave",
                City = "Springfield",
                Region = "IL",
                PostalCode = "62701",
                Country = "USA",
                Phone = "555-0200"
            },
            new() {
                CompanyName = "Initech",
                ContactName = "Bill Lumbergh",
                Address = "789 Corporate Blvd",
                City = "Austin",
                Region = "TX",
                PostalCode = "73301",
                Country = "USA",
                Phone = "555-0300"
            }
        };

        await context.Customers.AddRangeAsync(customers);
        await context.SaveChangesAsync();
    }
}