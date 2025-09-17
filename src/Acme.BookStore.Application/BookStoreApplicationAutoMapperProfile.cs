using Acme.BookStore.Books;
using Acme.BookStore.Authors;
using AutoMapper;

namespace Acme.BookStore;

public class BookStoreApplicationAutoMapperProfile : Profile
{
    public BookStoreApplicationAutoMapperProfile()
    {
        CreateMap<Book, BookDto>()
            .ForMember(dest => dest.AuthorName,
               opt => opt.MapFrom(src => src.Author != null ? src.Author.Name : string.Empty));

        CreateMap<CreateBookDto, Book>();
        CreateMap<UpdateBookDto, Book>();

        CreateMap<Author, AuthorDto>();
        CreateMap<Author, AuthorLookupDto>();

    }
}
