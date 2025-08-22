using Microsoft.EntityFrameworkCore;
using TodoApp.DataAccess.Implementations.TodoRepo;
using TodoApp.DataAccess.Interfaces.TodoRepo;
using TodoApp.Domain.Database;

namespace TodoApp.DataAccess
{
    public static class RepositoryInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetSection("todoConn").Value!;

            //Database Provider
            services.AddDbContext<AppDbContext>(x => x.UseSqlServer(connectionString));

            // Register repositories
            services.AddTransient<ITodoRepository, TodoRepository>();

            return services;
        }
    }
}
