using AutoMapper;
using TodoApp.Domain.Entities;
using TodoApp.Models.Todo;

namespace TodoApp.Configurations.AutoMapper.TodoMapper
{
    public class TodoDtoMapper : Profile
    {
        public TodoDtoMapper()
        {
            CreateMap<Todo, TodoCreateDto>().ReverseMap();
            CreateMap<Todo, TodoUpdateDto>().ReverseMap();
        }
    }
}
