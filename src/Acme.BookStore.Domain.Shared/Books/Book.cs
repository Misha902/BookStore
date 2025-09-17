using Acme.BookStore.Authors;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.BookStore.Books;

public class Book : AuditedAggregateRoot<Guid>
{
    public string Name { get; private set; }

    public BookType Type { get; set; }

    public DateTime PublishDate { get; set; }

    public float Price { get; set; }

    public Guid AuthorId { get; set; }

    public int Quantity { get; private set; } = 0;

    [NotMapped]
    public Author Author { get; set; }

    public Book(string name, BookType type, DateTime publishDate, float price, Guid authorId, int quantity)
    {
        SetName(name);
        SetQuantity(quantity);
        Type = type;
        PublishDate = publishDate;
        Price = price;
        AuthorId = authorId;
    }
    public void SetQuantity(int quantity)
    {
        if (quantity < 0)
        {
            throw new BusinessException();
        }

        Quantity = quantity;
    }

    public void SetName(string name)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));
        Name = name;
    }
}
