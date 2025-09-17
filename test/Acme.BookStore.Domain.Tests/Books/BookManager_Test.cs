using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Xunit;

namespace Acme.BookStore.Books
{
    public class BookManager_Test
    {
        private readonly BookManager _bookManager;
        private readonly Mock<IBookRepository> _bookRepoMock;

        public BookManager_Test() 
        { 
            _bookRepoMock = new Mock<IBookRepository>();
            //_bookManager = new BookManager(_bookRepoMock.Object);
        }

        [Fact]
        public async Task CreateAsync_Should_Return_Book_When_Name_Not_Exists()
        {
            var name = "Unique Book";
            _bookRepoMock.Setup(r => r.FindByNameAsync(name)).ReturnsAsync((Book?)null);

            var book = await _bookManager.CreateAsync(
                name,
                BookType.Science,
                new DateTime(2021, 1, 1),
                9.99f,
                Guid.NewGuid(),
                10
            );

            Assert.NotNull(book);
            Assert.Equal(name, book.Name);
            Assert.Equal(10, book.Quantity);
            _bookRepoMock.Verify(r => r.FindByNameAsync(name), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_Should_Throw_When_Name_Exists()
        {
            var name = "Existing Book";
            var existing = new Book(name, BookType.Science, DateTime.Now, 5f, Guid.NewGuid(), 1);
            _bookRepoMock.Setup(r => r.FindByNameAsync(name)).ReturnsAsync(existing);

            await Assert.ThrowsAsync<BusinessException>(async () =>
            {
                await _bookManager.CreateAsync(
                    name,
                    BookType.Science,
                    new DateTime(2021, 1, 1),
                    9.99f,
                    Guid.NewGuid(),
                    10
                );
            });

            _bookRepoMock.Verify(r => r.FindByNameAsync(name), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_When_Name_Not_Conflicting()
        {
            var book = new Book("OldName", BookType.Science, new DateTime(2020, 1, 1), 4.5f, Guid.NewGuid(), 2);
            var newName = "NewName";

            _bookRepoMock.Setup(r => r.FindByNameAsync(newName)).ReturnsAsync((Book?)null);

            var updated = await _bookManager.UpdateAsync(
                book,
                newName,
                BookType.Fantastic,
                new DateTime(2022, 1, 1),
                14.99f,
                Guid.NewGuid(),
                7
            );

            Assert.Same(book, updated);
            Assert.Equal(newName, book.Name);
            Assert.Equal(BookType.Fantastic, book.Type);
            Assert.Equal(7, book.Quantity);
            _bookRepoMock.Verify(r => r.FindByNameAsync(newName), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_Should_Throw_When_NewName_Conflicts()
        {
            var book = new Book("Original", BookType.Science, DateTime.Now, 3f, Guid.NewGuid(), 1);
            var newName = "ConflictingName";
            var other = new Book(newName, BookType.Science, DateTime.Now, 2f, Guid.NewGuid(), 1);

            _bookRepoMock.Setup(r => r.FindByNameAsync(newName)).ReturnsAsync(other);

            await Assert.ThrowsAsync<BusinessException>(async () =>
            {
                await _bookManager.UpdateAsync(
                    book,
                    newName,
                    BookType.Science,
                    DateTime.Now,
                    3f,
                    Guid.NewGuid(),
                    2
                );
            });

            _bookRepoMock.Verify(r => r.FindByNameAsync(newName), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_Should_Not_Check_Unique_When_Name_Not_Changed()
        {
            var book = new Book("SameName", BookType.Science, DateTime.Now, 3f, Guid.NewGuid(), 1);
            var sameName = "SameName";

            _bookRepoMock.Setup(r => r.FindByNameAsync(It.IsAny<string>())).Throws(new Exception("Should not be called"));

            var updated = await _bookManager.UpdateAsync(
                book,
                sameName,
                BookType.Fantastic,
                DateTime.Now,
                10f,
                Guid.NewGuid(),
                5
            );

            Assert.Equal(sameName, book.Name);
            Assert.Equal(BookType.Fantastic, book.Type);
            Assert.Equal(5, book.Quantity);
        }
    }
}
