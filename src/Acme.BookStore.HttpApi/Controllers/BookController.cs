using Acme.BookStore.Authors;
using Acme.BookStore.Books;
using Acme.BookStore.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Controllers;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace Acme.BookStore.Controllers
{
    [RemoteService(Name="BookStore")]
    [Route("api/app/mycontroller/book")]
    public class BookController : BookStoreController
    {
        private readonly IBookAppService _bookAppService;
        private readonly IAuthorAppService _authorAppService;

        public BookController(IBookAppService bookAppService, IAuthorAppService authorAppService)
        {
            _bookAppService = bookAppService;
            _authorAppService = authorAppService;
        }

        [HttpGet("{id}")]
        [Authorize(BookStorePermissions.Books.Default)]
        public async Task<BookDto> GetAsync(Guid id)
        {
            return await _bookAppService.GetAsync(id);
        }

        [HttpGet()]
        public async Task<PagedResultDto<BookDto>> GetListAsync([FromQuery] GetBookListDto input)
        {
            return await _bookAppService.GetListAsync(input);
        }

        [HttpGet("booksntr")]
        public async Task<PagedResultDto<BookDto>> GetListNoTrackAsync([FromQuery] GetBookListDto input)
        {
            return await _bookAppService.GetListNoTrackAsync(input);
        }

        [HttpPost]
        [Authorize(BookStorePermissions.Books.Create)]
        public async Task<BookDto> CreateAsync([FromBody] CreateBookDto input)
        {
            return await _bookAppService.CreateAsync(input);
        }

        [HttpPut("{id}")]
        [Authorize(BookStorePermissions.Books.Edit)]
        public async Task UpdateAsync(Guid id, [FromBody] UpdateBookDto input)
        {
           await _bookAppService.UpdateAsync(id, input);
        }

        [HttpDelete("{id}")]
        [Authorize(BookStorePermissions.Books.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _bookAppService.DeleteAsync(id);
        }

        [HttpGet("author-lookup")]
        public async Task<ListResultDto<AuthorLookupDto>> GetAuthorLookup()
        {
           return await _authorAppService.GetAuthorLookupAsync();
        }
    }
}
