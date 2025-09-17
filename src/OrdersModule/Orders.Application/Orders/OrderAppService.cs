using Abp.Application.Services;
using Acme.BookStore.Books;
using Microsoft.AspNetCore.Mvc;
using Orders.Application.Contracts.Order;
using Orders.Application.Contracts.Order.Dtos;
using Orders.Domain.Orders;
using Orders.Domain.Shared.OrderItems;
using Orders.Domain.Shared.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using ApplicationService = Volo.Abp.Application.Services.ApplicationService;

namespace Orders.Application.Orders
{
    [RemoteService(false)]
    public class OrderAppService : ApplicationService, IOrderAppService
    {
        private readonly OrderManager _orderManager;
        private readonly BookManager _bookManager;

        public OrderAppService(OrderManager orderManager, BookManager bookManager)
        {
            _orderManager = orderManager;
            _bookManager = bookManager;
        }

        public async Task<OrderDto> GetAsync(Guid id)
        {
            var order = await _orderManager.GetWithItemAsync(id);
            var dto = ObjectMapper.Map<Order, OrderDto>(order);

            foreach (var itemDto in dto.OrderItems)
            {
                var book = await _bookManager.GetAsync(itemDto.BookId);
                itemDto.AuthorName = book?.Author?.Name;
            }

            //foreach (var itemDto in dto.OrderItems)
            //{
            //    itemDto.AuthorName = order.OrderItems
            //        .First(x => x.BookId == itemDto.BookId)
            //        .Book?.Author?.Name;
            //}

            return dto;
        }


        public async Task<PagedResultDto<OrderDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var orders = await _orderManager.GetListWithItemsAsync(input.SkipCount, input.MaxResultCount, input.Sorting);
            var totalCount = await _orderManager.GetCountAsync();

            return new PagedResultDto<OrderDto>(
                totalCount,
                ObjectMapper.Map<List<Order>, List<OrderDto>>(orders)
            );
        }

        public async Task<OrderDto> CreateAsync(OrderDto input)
        {
            OrderStatus status = input.Status;
            var order = ObjectMapper.Map<OrderDto, Order>(input);

            var updatedOrder = await _orderManager.CreateAsync(order);
            return ObjectMapper.Map<Order, OrderDto>(updatedOrder);
        }

        public async Task<OrderDto> UpdateAsync(OrderDto input)
        {
            var order = ObjectMapper.Map<OrderDto, Order>(input);

            var updatedOrder = await _orderManager.UpdateAsync(order);
            return ObjectMapper.Map<Order, OrderDto>(updatedOrder);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _orderManager.DeleteAsync(id);
        }

        public async Task<OrderDto> CancelAsync(Guid id)
        {
            var order = await _orderManager.CancelAsync(id);
            return ObjectMapper.Map<Order, OrderDto>(order);
        }
    }
}