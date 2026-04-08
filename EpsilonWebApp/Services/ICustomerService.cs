using EpsilonWebApp.Shared.Models.DTO;

namespace EpsilonWebApp.Services;

public interface ICustomerService
{
    Task<(IEnumerable<CustomerDto> Items, int TotalCount)> GetPagedAsync(int page, int pageSize);
    Task<CustomerDto?> GetByIdAsync(Guid id);
    Task<CustomerDto> CreateAsync(CreateCustomerDto dto);
    Task<CustomerDto?> UpdateAsync(Guid id, UpdateCustomerDto dto);
    Task<bool> DeleteAsync(Guid id);
}