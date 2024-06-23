using AutoMapper;
using ContactsWebApp.Server.Models;
using ContactsWebApp.Shared.DTOs;

namespace ContactsWebApp.Server.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>()
                .ReverseMap();

            CreateMap<Contact, ContactDto>()
                .ReverseMap();

            CreateMap<RegisterRequestDto, User>();
        }
    }
}
