using TodoApp.DataAccess.Interfaces.TodoRepo;
using TodoApp.DataAccess.QuerySpecs;
using TodoApp.Domain.Entities;
using TodoApp.Models.Pagination;
using TodoApp.Services.Interfaces.TodoService;
using TodoApp.Utiities;

namespace TodoApp.Services.Implementations.TodoService
{
    public class TodoService : GenericService<Todo, Guid>, ITodoService
    {
        private readonly ITodoRepository _todoRepository;
        public TodoService(ITodoRepository todoRepository) : base(todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<PagedResponse<Todo>> GetPaginatedListAsync(TodoSpec spec)
        {
            var todos = _todoRepository.GetQueryable();
            //todos = ValidateQueryTimeParams(spec, todos); // Validate If Any attribute except String
            var queryBuilder = new QueryBuilder<Todo>(todos, spec)
            .AddSearch()
            .AddFilters()
            .AddSort();
            return await queryBuilder.BuildAsync();
        }
        //Validate If Any attribute except String
        //private static IQueryable<Todo> ValidateQueryTimeParams(TodoSpec spec, IQueryable<Todo> todos)
        //{
        //    if (spec.CreatedAt.HasValue && spec.CreatedAt.HasValue)
        //    {
        //        var from = spec.CreatedAt.Value.AddHours(6);
        //        var to = spec.CreatedAt.Value.AddHours(6);


        //        todos = todos.Where(a => a.CreatedAt.TimeOfDay >= from.TimeOfDay && a.CreatedAt.TimeOfDay <= to.TimeOfDay);
        //    }

        //    return todos;
        //}
    }
}
