using Orders.Domain.Shared.Orders;
using Volo.Abp.Application.Dtos;


namespace Orders.Application.Contracts.Order.Dtos
{
    public class OrderDto : FullAuditedEntityDto<Guid>
    {
        public string Address { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }

        public float TotalPrice { get; set; }
    }
}
