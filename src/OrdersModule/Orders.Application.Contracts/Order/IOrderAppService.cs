using Orders.Application.Contracts.Order.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Orders.Application.Contracts.Order
{
    public interface IOrderAppService : IApplicationService
    {
        Task<OrderDto> GetAsync(Guid id);
        Task<PagedResultDto<OrderDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        Task<OrderDto> CreateAsync(OrderDto input);
        Task<OrderDto> UpdateAsync(OrderDto input);
        Task DeleteAsync(Guid id);
        Task<OrderDto> CancelAsync(Guid id);
    }
}
