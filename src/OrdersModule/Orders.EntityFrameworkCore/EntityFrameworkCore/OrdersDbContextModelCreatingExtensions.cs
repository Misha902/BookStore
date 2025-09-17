using Acme.BookStore;
using Acme.BookStore.Books;
using Microsoft.EntityFrameworkCore;
using Orders.Domain.Shared.OrderItems;
using Orders.Domain.Shared.Orders;
using System.Reflection.Emit;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Orders.EntityFrameworkCore.EntityFrameworkCore;

public static class OrdersDbContextModelCreatingExtensions
{
    public static void ConfigureOrders(this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<Order>(b =>
        {
            b.ToTable("Orders");
            b.ConfigureByConvention();

            b.Property(o => o.Address)
                .IsRequired()
                .HasMaxLength(256);

            b.Property(o => o.Status)
                .IsRequired();

            b.Property(o => o.TotalPrice)
                .HasColumnType("real");

            b.HasMany(o => o.OrderItems)
             .WithOne()
             .HasForeignKey(oi => oi.OrderId)
             .IsRequired()
             .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<OrderItem>(b =>
        {
            b.ToTable("OrderItems");
            b.ConfigureByConvention();

            b.Property(oi => oi.Quantity)
                .IsRequired();

            b.Property(oi => oi.BookName)
                .IsRequired()
                .HasMaxLength(256);

            b.Property(o => o.BookPrice)
                .HasColumnType("real");

            b.Property(oi => oi.BookId)
                .IsRequired();
        });


    }
}

