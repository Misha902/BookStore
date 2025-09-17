using Acme.BookStore.Books;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

public class OrderItem : Entity<Guid>
{
    public Guid OrderId { get; private set; }
    public Guid BookId { get; private set; }
    [NotMapped] public string BookName { get; set; }
    [NotMapped] public float BookPrice { get; set; }
    public int Quantity { get; private set; }

    private OrderItem() { }

    public OrderItem(Guid id, Guid orderId, Guid bookId, int quantity, float price, string bookName)
        : base(id)
    {
        OrderId = orderId;
        BookId = bookId;
        SetQuantity(quantity);
        SetPrice(price);
        BookName = bookName;
     }

    public void SetQuantity(int quantity)
    {
        Check.Positive(quantity, nameof(quantity));
        Quantity = quantity;
    }

    public void SetPrice(float price)
    {
        Check.Positive(price, nameof(price));
        BookPrice = price;
    }
}
