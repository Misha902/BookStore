using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore.Books
{
    public interface IBookRepository : IRepository<Book, Guid>
    {
        Task<Book?> FindByNameAsync(string name);
        Task<List<Book>> GetListByAuthorAsync(Guid authorId);
        Task<List<Book>> GetListByPriceRangeAsync(float minPrice, float maxPrice);

        Task<List<Book>> GetListNoTrackAsync();

        Task<Book> GetNoTrackAsync(Guid id);
    }
}
