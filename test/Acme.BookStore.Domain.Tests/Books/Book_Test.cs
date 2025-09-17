using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Xunit;

namespace Acme.BookStore.Books
{
    public class Book_Test
    {
        [Fact]
        public void SetQuantity_Should_Set_When_NotNegative()
        {
            var book = new Book("Test",
                                        BookType.Science,
                                        new DateTime(2020, 1, 1),
                                        10f, Guid.NewGuid(), 1);

            book.SetQuantity(5);

            Assert.Equal(5, book.Quantity);
        }

        [Fact]
        public void SetQuantity_Should_Throw_BussinessException_When_Negative()
        {
            var book = new Book("Test",
                                        BookType.Science,
                                        new DateTime(2020, 1, 1),
                                        10f, Guid.NewGuid(), 1);

            Assert.Throws<BusinessException>(() => book.SetQuantity(-3));
        }

        [Fact]
        public void SetName_Should_Throw_When_Null_Or_Whitespace()
        {
            var book = new Book("Initial", BookType.Science, DateTime.Now, 10f, Guid.NewGuid(), 0);

            Assert.Throws<ArgumentException>(() => book.SetName(null!));
            Assert.Throws<ArgumentException>(() => book.SetName(string.Empty));
            Assert.Throws<ArgumentException>(() => book.SetName("   "));
        }

        [Fact]
        public void Constructor_Should_Set_Properties()
        {
            var name = "My Book";
            var type = BookType.Science;
            var publish = new DateTime(2000, 1, 1);
            var price = 12.5f;
            var authorId = Guid.NewGuid();
            var quantity = 3;

            var book = new Book(name, type, publish, price, authorId, quantity);

            Assert.Equal(name, book.Name);
            Assert.Equal(type, book.Type);
            Assert.Equal(publish, book.PublishDate);
            Assert.Equal(price, book.Price);
            Assert.Equal(authorId, book.AuthorId);
            Assert.Equal(quantity, book.Quantity);
        }
    }
}
