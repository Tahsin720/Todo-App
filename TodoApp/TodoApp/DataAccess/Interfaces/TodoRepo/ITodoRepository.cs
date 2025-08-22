using TodoApp.Domain.Entities;

namespace TodoApp.DataAccess.Interfaces.TodoRepo;

public interface ITodoRepository : IGenericRepository<Todo, Guid>
{
    IQueryable<Todo> GetQueryable();
}
