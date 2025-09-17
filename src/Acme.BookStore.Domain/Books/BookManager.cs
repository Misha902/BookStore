using Acme.BookStore.Authors;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Acme.BookStore.Books
{
    public class BookManager : DomainService
    {
        private readonly IBookRepository _bookRepository;
        private readonly AuthorManager _authorManager;
        public BookManager(IBookRepository bookRepository, AuthorManager authorManager) 
        { 
            _bookRepository = bookRepository;
            _authorManager = authorManager;
        }

        public async Task<Book> GetAsync(Guid id)
        {
            var book = await _bookRepository.GetAsync(id);
            book.Author = await _authorManager.GetAsync(book.AuthorId);
            return book;
        }

        public async Task<Author> GetAuthorAsync(Guid authorId)
        {
            return await _authorManager.GetAsync(authorId);
        }

        public async Task<Book> GetNoTrackAsync(Guid id)
        {
            return await _bookRepository.GetNoTrackAsync(id);
        }

        public async Task<List<Book>> GetListNoTrackAsync()
        {
            return await _bookRepository.GetListNoTrackAsync();
        }

        public async Task<List<Book>> GetListAsync()
        {
            return await _bookRepository.GetListAsync();
        }
        public async Task<List<Book>> GetPagedListAsync(int skipCount, int maxResultCount, string v)
        {
            var books = await _bookRepository.GetPagedListAsync(skipCount, maxResultCount, v);

            foreach (var book in books)
            {
                book.Author = await _authorManager.GetAsync(book.AuthorId);
            }

            return books;
        }

        public async Task<Book> CreateAsync(string name,
                                            BookType type,
                                            DateTime publishDate,
                                            float price,
                                            Guid authorId,
                                            int quantity)
        {
            var author = await _authorManager.GetAsync(authorId);
            if (author == null)
            {
                throw new BusinessException("BookStore:AuthorNotFound")
                    .WithData("AuthorId", authorId);
            }

            var book = new Book(name, type, publishDate, price, authorId, quantity);
            book.Author = author;

            return await _bookRepository.InsertAsync(book);
        }

        public async Task<Book> UpdateAsync(Book book,
                                            string name,
                                            BookType type,
                                            DateTime publishDate,
                                            float price,
                                            Guid authorId,
                                            int quantity)
        {
            var author = await _authorManager.GetAsync(authorId);
            if (author == null)
            {
                throw new BusinessException("BookStore:AuthorNotFound")
                    .WithData("AuthorId", authorId);
            }

            book.SetName(name);
            book.Type = type;
            book.PublishDate = publishDate;
            book.Price = price;
            book.AuthorId = authorId;
            book.SetQuantity(quantity);
            book.Author = author;

            return book;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _bookRepository.DeleteAsync(id);
        }

        public async Task<long> GetCountAsync()
        {
            return await _bookRepository.GetCountAsync();
        }

        public async Task DecreaseQuantity(Book book, int quantity)
        {
            if (book.Quantity < quantity)
            {
                throw new BusinessException(BookStoreDomainErrorCodes.DontEnoughBooks);
            }
            book.SetQuantity(book.Quantity - quantity);

            await _bookRepository.UpdateAsync(book, autoSave: true);
        }
    }
}
