using Acme.BookStore.Authors;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.Books;

public interface IBookAppService : IApplicationService
{
    Task<BookDto> GetAsync(Guid id);

    Task<PagedResultDto<BookDto>> GetListAsync(PagedAndSortedResultRequestDto input);

    Task<PagedResultDto<BookDto>> GetListNoTrackAsync(PagedAndSortedResultRequestDto input);

    Task<BookDto> CreateAsync(CreateBookDto input);

    Task<BookDto> UpdateAsync(Guid id, UpdateBookDto input);

    Task DeleteAsync(Guid id);
}
