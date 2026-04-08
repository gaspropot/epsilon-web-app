using EpsilonWebApp.Shared.Models.DTO;

namespace EpsilonWebApp.Services;

// Hint: I did not create an interface for this as it works mostly as a facade between UI and services.
// It could also just not exist and the razor pages would just inject ICustomerService.
public class CustomerApiService
{
    private readonly ICustomerService _customerService;

    public CustomerApiService(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task<PagedResult<CustomerDto>?> GetPagedAsync(int page, int pageSize,
        CancellationToken cancellationToken = default)
    {
        var (items, total) = await _customerService.GetPagedAsync(page, pageSize);
        return new PagedResult<CustomerDto>
        {
            Items = items,
            TotalCount = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<bool> CreateAsync(CreateCustomerDto dto)
    {
        await _customerService.CreateAsync(dto);
        return true;
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateCustomerDto dto)
    {
        var result = await _customerService.UpdateAsync(id, dto);
        return result is not null;
    }

    public async Task<bool> DeleteAsync(Guid id)
        => await _customerService.DeleteAsync(id);
}

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = [];
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}