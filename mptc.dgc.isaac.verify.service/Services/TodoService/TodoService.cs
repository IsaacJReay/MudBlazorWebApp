
using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using mptc.dgc.isaac.verify.dal.Models;
using mptc.dgc.isaac.verify.service.Dtos.Commons;
using mptc.dgc.isaac.verify.service.Dtos.Todo;
using mptc.dgc.isaac.verify.service.Repositories;

namespace mptc.dgc.isaac.verify.service.Services.TodoService
{
    public class TodoService : ITodoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TodoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<TodoDto> CreateAsync(TodoDto todoItem)
        {
            if (todoItem != null)
            {
                var todoEntity = _mapper.Map<Todo>(todoItem);
                await _unitOfWork.Repository<Todo, int>().CreateAsync(todoEntity);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<TodoDto>(todoEntity);
            }
            throw new ArgumentNullException(nameof(todoItem));
        }

        public async Task<TodoDto> CreateTransactionAsync(TodoDto todoItem)
        {
            if (todoItem != null)
            {
                var result = new TodoDto();
                await _unitOfWork.Repository<Todo, int>().ExecuteInTransactionAsync(
                    async () =>
                    {
                        var todoEntity = _mapper.Map<Todo>(todoItem);
                        await _unitOfWork.Repository<Todo, int>().CreateAsync(todoEntity);
                        await _unitOfWork.SaveChangesAsync();
                        result = _mapper.Map<TodoDto>(todoEntity);
                    }
                );
                return result;
            }
            throw new ArgumentNullException(nameof(todoItem));
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _unitOfWork.Repository<Todo, int>().DeleteAsync(id);
            if (result)
            {
                await _unitOfWork.SaveChangesAsync();
            }
            return result;
        }

        public async Task<IEnumerable<TodoDto>> GetAllAsync()
        {
            var todos = await _unitOfWork.Repository<Todo, int>().GetAllAsync();
            return await todos.ProjectTo<TodoDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<TodoDto> GetByIdAsync(int id)
        {
            var todoEntity = await _unitOfWork.Repository<Todo, int>().GetByIdAsync(id);
            return _mapper.Map<TodoDto>(todoEntity);
        }

        public async Task<PaginationResultDto<TodoDto>> GetPagedAsync(PaginationFilterDto paginationFilter, TodoDtoFilterDto? todoDtoFilter = null)
        {
            Expression<Func<Todo, bool>>? filter = null;

            if (todoDtoFilter != null)
            {
                if (!string.IsNullOrEmpty(todoDtoFilter.SearchText))
                {
                    filter = todo => todo.Title.Contains(todoDtoFilter.SearchText) || todo.Description.Contains(todoDtoFilter.SearchText);
                }
            }

            var pagedResult = await _unitOfWork.Repository<Todo, int>().GetPagedAsync(paginationFilter.PageNumber, paginationFilter.PageSize, filter);
            var todoDtos = _mapper.Map<List<TodoDto>>(pagedResult.Data);

            return new PaginationResultDto<TodoDto>(paginationFilter.PageSize, pagedResult.TotalCount, paginationFilter.PageNumber, todoDtos);
        }

        public async Task<bool> UpdateAsync(TodoDto todoItem)
        {
            var todoEntity = await _unitOfWork.Repository<Todo, int>().GetByIdAsync(todoItem.Id, false);
            if (todoEntity == null)
            {
                return false;
            }

            _mapper.Map(todoItem, todoEntity);
            await _unitOfWork.Repository<Todo, int>().UpdateAsync(todoEntity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}