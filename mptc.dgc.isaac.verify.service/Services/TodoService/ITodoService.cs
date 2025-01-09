
using mptc.dgc.isaac.verify.service.Dtos.Commons;
using mptc.dgc.isaac.verify.service.Dtos.Todo;

namespace mptc.dgc.isaac.verify.service.Services.TodoService
{
    public interface ITodoService
    {
        Task<PaginationResultDto<TodoDto>> GetPagedAsync(PaginationFilterDto paginationFilterDto, TodoDtoFilterDto? todoDtoFilterDto = null); // Retrieve all to-do items
        Task<IEnumerable<TodoDto>> GetAllAsync();
        Task<TodoDto> GetByIdAsync(int id);
        Task<TodoDto> CreateAsync(TodoDto todoItem);
        Task<TodoDto> CreateTransactionAsync(TodoDto todoItem);
        Task<bool> UpdateAsync(TodoDto todoItem);
        Task<bool> DeleteAsync(int id);
    }
}