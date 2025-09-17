using Acme.BookStore.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Acme.BookStore.Authors;

public class AuthorManager : DomainService
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorManager(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<Author> GetAsync(Guid authorId)
    {
        return await _authorRepository.GetAsync(authorId);
    }

    public async Task<List<Author>> GetListAsync()
    {
        return await _authorRepository.GetListAsync();
    }

    public async Task<List<Author>> GetPagedListAsync(int skipCount, int maxResultCount, string v)
    {
        return await _authorRepository.GetPagedListAsync(skipCount, maxResultCount, v);
    }

    public async Task<Author> CreateAsync(
        string name,
        DateTime birthDate,
        string? shortBio = null)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));

        var existingAuthor = await _authorRepository.FindByNameAsync(name);
        if (existingAuthor != null)
        {
            throw new AuthorAlreadyExistsException(name);
        }

        var author = new Author(
            GuidGenerator.Create(),
            name,
            birthDate,
            shortBio
        );

        return await _authorRepository.InsertAsync(author);
    }

    public async Task ChangeNameAsync(
        Author author,
        string newName)
    {
        Check.NotNull(author, nameof(author));
        Check.NotNullOrWhiteSpace(newName, nameof(newName));

        var existingAuthor = await _authorRepository.FindByNameAsync(newName);
        if (existingAuthor != null && existingAuthor.Id != author.Id)
        {
            throw new AuthorAlreadyExistsException(newName);
        }

        author.ChangeName(newName);
    }

    public async Task<long> CountAsync()
    {
        return await _authorRepository.GetCountAsync();
    }
    public async Task<long> CountAsync(Func<Author, bool> predicate)
    {
        var allAuthors = await _authorRepository.GetListAsync();
        return allAuthors.Count(predicate);
    }

    public async Task UpdateAsync(Author author)
    {
        await _authorRepository.UpdateAsync(author);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _authorRepository.DeleteAsync(id);
    }
}
