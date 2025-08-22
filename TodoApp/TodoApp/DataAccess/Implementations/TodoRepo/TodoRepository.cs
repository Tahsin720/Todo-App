using TodoApp.DataAccess.Interfaces.TodoRepo;
using TodoApp.Domain.Database;
using TodoApp.Domain.Entities;

namespace TodoApp.DataAccess.Implementations.TodoRepo
{
    public class TodoRepository : GenericRepository<Todo, Guid>, ITodoRepository
    {
        public readonly AppDbContext _db;
        public TodoRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public IQueryable<Todo> GetQueryable()
        {
            return _db.Todos.AsQueryable();
        }
    }
}
