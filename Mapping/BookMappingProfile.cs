using AutoMapper;
using WebApiHome4.DTOs;
using WebApiHome4.Models;

namespace WebApiHome4.Mapping;

public class BookMappingProfile : Profile
{
    public BookMappingProfile()
    {
        CreateMap<Book, BookDto>();
        CreateMap<CreateBookDto, Book>();
        CreateMap<UpdateBookDto, Book>();
    }
}
