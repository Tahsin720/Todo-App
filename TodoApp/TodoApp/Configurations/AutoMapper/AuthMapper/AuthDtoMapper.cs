using AutoMapper;
using TodoApp.Domain.Entities;
using TodoApp.Models.Auth;

namespace TodoApp.Configurations.AutoMapper.AuthMapper
{
    public class AuthDtoMapper : Profile
    {
        public AuthDtoMapper()
        {
            CreateMap<User, UserCreateDto>().ReverseMap();
        }
    }
}
