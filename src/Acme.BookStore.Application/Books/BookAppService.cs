using Acme.BookStore.Authors;
using Acme.BookStore.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.Books;

[RemoteService(false)]
[Authorize(BookStorePermissions.Books.Default)]
public class BookAppService : ApplicationService, IBookAppService
{
    private readonly BookManager _bookManager;

    public BookAppService(BookManager bookManager)
    {
        _bookManager = bookManager;
    }

    public async Task<BookDto> GetAsync(Guid id)
    {
        var book = await _bookManager.GetAsync(id);
       // book.Author = await _bookManager.GetAuthorAsync(book.AuthorId);
        var dto = ObjectMapper.Map<Book, BookDto>(book);

        return dto;
    }

    [Authorize(BookStorePermissions.Books.Create)]
    public async Task<BookDto> CreateAsync(CreateBookDto input)
    {
        var book = await _bookManager.CreateAsync(
            input.Name,
            input.Type,
            input.PublishDate,
            input.Price,
            input.AuthorId,
            input.Quantity
        );

        return ObjectMapper.Map<Book, BookDto>(book);
    }

    [Authorize(BookStorePermissions.Books.Edit)]
    public async Task<BookDto> UpdateAsync(Guid id, UpdateBookDto input)
    {
        var book = await _bookManager.GetAsync(id);

        await _bookManager.UpdateAsync(
            book,
            input.Name,
            input.Type,
            input.PublishDate,
            input.Price,
            input.AuthorId,
            input.Quantity
        );

        return ObjectMapper.Map<Book, BookDto>(book);
    }

    [Authorize(BookStorePermissions.Books.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _bookManager.DeleteAsync(id);
    }

    public async Task<PagedResultDto<BookDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        var totalCount = await _bookManager.GetCountAsync();
        var books = await _bookManager.GetPagedListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting ?? nameof(Book.Name)
        );

        var dtos = ObjectMapper.Map<List<Book>, List<BookDto>>(books);
        return new PagedResultDto<BookDto>(totalCount, dtos);
    }

    public async Task<PagedResultDto<BookDto>> GetListNoTrackAsync(PagedAndSortedResultRequestDto input)
    {
        var totalCount = await _bookManager.GetCountAsync();
        var books = await _bookManager.GetListNoTrackAsync();

        var dtos = ObjectMapper.Map<List<Book>, List<BookDto>>(books);
        return new PagedResultDto<BookDto>(totalCount, dtos);
    }
}