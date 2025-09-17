using Acme.BookStore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Acme.BookStore.Books
{
    internal class EfCoreBookRepository : EfCoreRepository<BookStoreDbContext, Book, Guid>, IBookRepository
    {
        public EfCoreBookRepository(IDbContextProvider<BookStoreDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<Book> GetNoTrackAsync(Guid id)
        {
            var dbSet = await GetDbSetAsync();
            var book = await dbSet.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);

            return book;
        }

        public async Task<List<Book>> GetListNoTrackAsync()
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<Book?> FindByNameAsync(string name)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.FirstOrDefaultAsync(book => book.Name == name);
        }

        public async Task<List<Book>> GetListByAuthorAsync(Guid authorId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.Where(book => book.AuthorId == authorId)
                              .ToListAsync();
        }

        public async Task<List<Book>> GetListByPriceRangeAsync(float minPrice, float maxPrice)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(book => book.Price >= minPrice && book.Price <= maxPrice)
                .OrderBy(book => book.Price)
                .ToListAsync();
        }
    }
}
