using AutoMapper;
using Orders.Application.Contracts.Order.Dtos;
using Orders.Domain.Shared.OrderItems;
using Orders.Domain.Shared.Orders;

namespace Orders.Application;

public class OrdersApplicationAutoMapperProfile : Profile
{
    public OrdersApplicationAutoMapperProfile()
    {
        CreateMap<Order, OrderDto>();
        CreateMap<OrderDto, Order>();
        CreateMap<OrderItem, OrderItemDto>();
        CreateMap<OrderItemDto, OrderItem>();
    }
}
