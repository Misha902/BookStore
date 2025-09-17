using Acme.BookStore.Books;
using Orders.Domain.Shared.OrderItems;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace Orders.Domain.Shared.Orders;

public class Order : FullAuditedAggregateRoot<Guid>
{
    public string Address { get; set; }
    public OrderStatus Status { get; set; }
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public float TotalPrice { get; set; } = 0;

    private Order() 
    {

    }

    public Order(Guid id, string address, OrderStatus status)
        : base(id)
    {
        ChangeAddress(address);
        Status = status;
        OrderItems = new List<OrderItem>();
    }

    public void AddItem(Book book, int quantity)
    {
        Check.Positive(book.Price, nameof(book.Price));

        var orderItem = new OrderItem(Guid.NewGuid(), Id, book.Id, quantity, book.Price, book.Name);
        OrderItems.Add(orderItem);
        TotalPrice += book.Price * quantity;
    }

    public void UpdateItem(Guid id, Guid bookId, float bookPrice, string bookName, int quantity)
    {
        
    }


    public void ChangeAddress(string address)
    {
        Check.NotNullOrWhiteSpace(address, nameof(address));
        Address = address;
    }

    public void ChangeStatus(OrderStatus status)
    {
        Status = status;
    }

    public void RemoveItem(Guid orderItemId)
    {
        var item = OrderItems.FirstOrDefault(x => x.Id == orderItemId);
        if (item == null)
        {
            throw new BusinessException("Order.ItemNotFound")
                .WithData("OrderItemId", orderItemId);
        }

        OrderItems.Remove(item);
    }

    public void ClearItems()
    {
        OrderItems.Clear(); 
    }
}
