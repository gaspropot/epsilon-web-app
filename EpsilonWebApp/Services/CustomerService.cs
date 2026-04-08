using Microsoft.EntityFrameworkCore;
using EpsilonWebApp.Data;
using EpsilonWebApp.Models;
using EpsilonWebApp.Shared.Models.DTO;

namespace EpsilonWebApp.Services;

public class CustomerService : ICustomerService
{
    private readonly AppDbContext _context;

    public CustomerService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<CustomerDto> Items, int TotalCount)> GetPagedAsync(int page, int pageSize)
    {
        var query = _context.Customers.AsNoTracking();
        var total = await query.CountAsync();
        var items = await query
            .OrderBy(c => c.CompanyName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => MapToDto(c))
            .ToListAsync();

        return (items, total);
    }

    public async Task<CustomerDto?> GetByIdAsync(Guid id)
    {
        var customer = await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        return customer is null ? null : MapToDto(customer);
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto)
    {
        var customer = new Customer
        {
            CompanyName = dto.CompanyName,
            ContactName = dto.ContactName,
            Address = dto.Address,
            City = dto.City,
            Region = dto.Region,
            PostalCode = dto.PostalCode,
            Country = dto.Country,
            Phone = dto.Phone
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return MapToDto(customer);
    }

    public async Task<CustomerDto?> UpdateAsync(Guid id, UpdateCustomerDto dto)
    {
        var existing = await _context.Customers.FindAsync(id);
        if (existing is null) return null;

        existing.CompanyName = dto.CompanyName;
        existing.ContactName = dto.ContactName;
        existing.Address = dto.Address;
        existing.City = dto.City;
        existing.Region = dto.Region;
        existing.PostalCode = dto.PostalCode;
        existing.Country = dto.Country;
        existing.Phone = dto.Phone;

        await _context.SaveChangesAsync();
        return MapToDto(existing);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existing = await _context.Customers.FindAsync(id);
        if (existing is null) return false;

        _context.Customers.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }

    private static CustomerDto MapToDto(Customer c) => new()
    {
        Id = c.Id,
        CompanyName = c.CompanyName,
        ContactName = c.ContactName,
        Address = c.Address,
        City = c.City,
        Region = c.Region,
        PostalCode = c.PostalCode,
        Country = c.Country,
        Phone = c.Phone
    };
}
