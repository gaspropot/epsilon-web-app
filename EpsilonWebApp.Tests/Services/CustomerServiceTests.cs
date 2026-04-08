using Microsoft.EntityFrameworkCore;
using EpsilonWebApp.Data;
using EpsilonWebApp.Models;
using EpsilonWebApp.Services;
using EpsilonWebApp.Shared.Models.DTO;

namespace EpsilonWebApp.Tests.Services;

public class CustomerServiceTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly CustomerService _service;

    public CustomerServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _service = new CustomerService(_context);
    }

    private async Task SeedCustomersAsync(int count)
    {
        var customers = Enumerable.Range(1, count).Select(i => new Customer
        {
            Id = Guid.NewGuid(),
            CompanyName = $"Company {i:D2}",
            ContactName = $"Contact {i}",
            City = $"City {i}",
            Country = "USA",
            Phone = $"555-{i:D4}"
        });

        await _context.Customers.AddRangeAsync(customers);
        await _context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetPagedAsync_ReturnsCorrectPage()
    {
        await SeedCustomersAsync(15);

        var (items, total) = await _service.GetPagedAsync(1, 10);

        Assert.Equal(10, items.Count());
        Assert.Equal(15, total);
    }

    [Fact]
    public async Task GetPagedAsync_SecondPage_ReturnsRemainingItems()
    {
        await SeedCustomersAsync(15);

        var (items, total) = await _service.GetPagedAsync(2, 10);

        Assert.Equal(5, items.Count());
        Assert.Equal(15, total);
    }

    [Fact]
    public async Task GetPagedAsync_ReturnsItemsOrderedByCompanyName()
    {
        await SeedCustomersAsync(5);

        var (items, _) = await _service.GetPagedAsync(1, 10);
        var names = items.Select(c => c.CompanyName).ToList();

        Assert.Equal(names.OrderBy(n => n).ToList(), names);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsCustomer()
    {
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            CompanyName = "Test Company"
        };
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();

        var result = await _service.GetByIdAsync(customer.Id);

        Assert.NotNull(result);
        Assert.Equal("Test Company", result.CompanyName);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        var result = await _service.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_AddsCustomerToDatabase()
    {
        var dto = new CreateCustomerDto
        {
            CompanyName = "New Company",
            ContactName = "John Doe",
            City = "New York",
            Country = "USA"
        };

        var result = await _service.CreateAsync(dto);

        Assert.NotNull(result);
        Assert.Equal("New Company", result.CompanyName);
        Assert.Equal(1, await _context.Customers.CountAsync());
    }

    [Fact]
    public async Task CreateAsync_GeneratesNewId()
    {
        var dto = new CreateCustomerDto { CompanyName = "Test" };

        var result = await _service.CreateAsync(dto);

        Assert.NotEqual(Guid.Empty, result.Id);
    }

    [Fact]
    public async Task UpdateAsync_ExistingId_UpdatesCustomer()
    {
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            CompanyName = "Old Name"
        };
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();

        var dto = new UpdateCustomerDto { CompanyName = "New Name" };
        var result = await _service.UpdateAsync(customer.Id, dto);

        Assert.NotNull(result);
        Assert.Equal("New Name", result.CompanyName);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingId_ReturnsNull()
    {
        var dto = new UpdateCustomerDto { CompanyName = "Test" };

        var result = await _service.UpdateAsync(Guid.NewGuid(), dto);

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_RemovesCustomer()
    {
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            CompanyName = "To Delete"
        };
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();

        var result = await _service.DeleteAsync(customer.Id);

        Assert.True(result);
        Assert.Equal(0, await _context.Customers.CountAsync());
    }

    [Fact]
    public async Task DeleteAsync_NonExistingId_ReturnsFalse()
    {
        var result = await _service.DeleteAsync(Guid.NewGuid());

        Assert.False(result);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}