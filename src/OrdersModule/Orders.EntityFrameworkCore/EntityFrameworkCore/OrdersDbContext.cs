using Microsoft.EntityFrameworkCore;
using Orders.Domain.Shared.OrderItems;
using Orders.Domain.Shared.Orders;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;


namespace Orders.EntityFrameworkCore.EntityFrameworkCore
{
    [ConnectionStringName("Orders")]
    public class OrdersDbContext : AbpDbContext<OrdersDbContext>
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public OrdersDbContext(DbContextOptions<OrdersDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureOrders();
        }
    }
}