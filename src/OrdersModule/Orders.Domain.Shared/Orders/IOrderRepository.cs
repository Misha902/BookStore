using Volo.Abp.Domain.Repositories;

namespace Orders.Domain.Shared.Orders
{
    public interface IOrderRepository : IRepository<Order, Guid>
    {
        Task<Order> GetWithItemsAsync(Guid id);
        Task<List<Order>> GetListWithItemsAsync(int skipCount, int maxResultCount, string sorting);
        
    }
}
