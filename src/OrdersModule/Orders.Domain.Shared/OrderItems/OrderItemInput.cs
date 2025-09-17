namespace Orders.Domain.Shared.OrderItems
{
    public record OrderItemInput(Guid bookId, int quantity);
}
