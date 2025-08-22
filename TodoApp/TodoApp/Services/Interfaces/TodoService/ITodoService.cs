using TodoApp.DataAccess.QuerySpecs;
using TodoApp.Domain.Entities;
using TodoApp.Models.Pagination;

namespace TodoApp.Services.Interfaces.TodoService
{
    public interface ITodoService : IGenericService<Todo, Guid>
    {
        Task<PagedResponse<Todo>> GetPaginatedListAsync(TodoSpec spec);
    }
}
