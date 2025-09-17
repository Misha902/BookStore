using Acme.BookStore.Books;
using Acme.BookStore.Controllers;
using Acme.BookStore.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Orders.Application.Contracts.Order;
using Orders.Application.Contracts.Order.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace Orders.HttpApi.Controllers
{
    //[RemoteService(Name = "BookStore")]
    [Route("api/order")]
    public class OrderController : AbpController
    {
        private readonly IOrderAppService _orderAppService;

        public OrderController(IOrderAppService orderAppService)
        {
            _orderAppService = orderAppService;
        }

        [HttpGet("{id}")]
        public async Task<OrderDto> GetAsync(Guid id)
        {
            return await _orderAppService.GetAsync(id);
        }

        [HttpGet]
        public async Task<PagedResultDto<OrderDto>> GetListAsync([FromQuery] PagedAndSortedResultRequestDto input)
        {
            return await _orderAppService.GetListAsync(input);
        }

        [HttpPost]
        public async Task<OrderDto> CreateAsync([FromBody] OrderDto input)
        {
            return await _orderAppService.CreateAsync(input);
        }

        [HttpPut("{id}")]
        public async Task<OrderDto> UpdateAsync([FromBody] OrderDto input)
        {
            return await _orderAppService.UpdateAsync(input);
        }

        [HttpPatch("{id}")]
        public async Task<OrderDto> CancelAsync(Guid id)
        {
            return await _orderAppService.CancelAsync(id);
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(Guid id)
        {
            await _orderAppService.DeleteAsync(id);
        }
    }
}
