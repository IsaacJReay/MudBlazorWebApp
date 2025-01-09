using AutoMapper;
using mptc.dgc.isaac.verify.dal.Models;
using mptc.dgc.isaac.verify.service.Dtos.Todo;

namespace mptc.dgc.isaac.verify.service.AutoMapper
{
    public class TodoMappingProfile : Profile
    {
        public TodoMappingProfile()
        {
            CreateMap<Todo, TodoDto>();
            CreateMap<TodoDto, Todo>();
        }
    }
}