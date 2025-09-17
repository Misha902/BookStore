using Acme.BookStore.Authors;
using Acme.BookStore.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace Acme.BookStore.Controllers
{
    [RemoteService(Name = "BookStore")]
    [Route("api/app/mycontroller/author")]
    public class AuthorController : BookStoreController
    {
        private readonly IAuthorAppService _authorAppService;

        public AuthorController(IAuthorAppService authorAppService)
        {
            _authorAppService = authorAppService;
        }

        [HttpGet("{id}")]
        public async Task<AuthorDto> GetAsync(Guid id)
        {
            return await _authorAppService.GetAsync(id);
        }

        [HttpGet]
        public async Task<PagedResultDto<AuthorDto>> GetListAsync([FromQuery] GetAuthorListDto input)
        {
            return await _authorAppService.GetListAsync(input);
        }

        [HttpPost]
        [Authorize(BookStorePermissions.Authors.Create)]
        public async Task<AuthorDto> CreateAsync([FromBody] CreateAuthorDto input)
        {
            return await _authorAppService.CreateAsync(input);
        }

        [HttpPut("{id}")]
        [Authorize(BookStorePermissions.Authors.Edit)]
        public async Task UpdateAsync(Guid id, [FromBody] UpdateAuthorDto input)
        {
            await _authorAppService.UpdateAsync(id, input);
        }

        [HttpDelete("{id}")]
        [Authorize(BookStorePermissions.Authors.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _authorAppService.DeleteAsync(id);
        }
    }
}
