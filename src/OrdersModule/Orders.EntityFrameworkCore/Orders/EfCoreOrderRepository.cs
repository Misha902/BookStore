using Acme.BookStore.Books;
using Microsoft.EntityFrameworkCore;
using Orders.Domain.Shared.Orders;
using Orders.EntityFrameworkCore.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Orders.EntityFrameworkCore.Orders
{
    public class EfCoreOrderRepository
    : EfCoreRepository<OrdersDbContext, Order, Guid>, IOrderRepository
    {
        public EfCoreOrderRepository(IDbContextProvider<OrdersDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<Order>> GetListWithItemsAsync(int skipCount, int maxResultCount, string sorting)
        {
            var dbContext = await GetDbContextAsync();

            var orders = await dbContext.Orders
                                    .Include(o => o.OrderItems)
                                    .OrderBy(o => o.CreationTime)
                                    .Skip(skipCount)
                                    .Take(maxResultCount)
                                    .ToListAsync();

            return orders;
        }

        public async Task<Order> GetWithItemsAsync(Guid id)
        {
            var dbContext = await GetDbContextAsync();
            var order = await dbContext.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            return order;
        }

        //public async Task<Order> GetWithItemsWithBooksAsync(Guid id)
        //{
        //    var dbContext = await GetDbContextAsync();
        //    var order = await dbContext.Orders
        //        .Include(o => o.OrderItems)
        //        .FirstOrDefaultAsync(o => o.Id == id);

        //    return order;
        //}

        //public async Task<Order> UpdateOrderAsync(Order changedOrder)
        //{
        //    var dbContext = await GetDbContextAsync();



        //    await dbContext.SaveChangesAsync();

        //    return changedOrder;

        //}

    }
}
