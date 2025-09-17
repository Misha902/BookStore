using Volo.Abp.Application.Dtos;

namespace Orders.Application.Contracts.Order.Dtos
{
    public class OrderItemDto : EntityDto<Guid>
    {
        public Guid OrderId { get; set; }
        public Guid BookId { get; set; }
        public int Quantity { get; set; }
        //public float TotalPrice { get; set; }
        public string AuthorName { get; set; }
        public string BookName { get; set; }
        public float BookPrice { get; set; }
    }
}
