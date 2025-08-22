using TodoApp.DataAccess;
using TodoApp.Services.Implementations.TodoService;
using TodoApp.Services.Implementations.Token;
using TodoApp.Services.Interfaces.TodoService;

namespace TodoApp.Services
{
    public static class ServiceInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRepositories(configuration);

            // Add your service registrations here
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<ITodoService, TodoService>();

            return services;
        }
    }
}
