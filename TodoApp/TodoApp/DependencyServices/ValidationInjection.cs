// Ignore Spelling: HRM Api

using FluentValidation;
using TodoApp.Configurations.Validation;
using TodoApp.Configurations.Validation.Auth;
using TodoApp.Configurations.Validation.Todo;
using TodoApp.Models.Auth;
using TodoApp.Models.Todo;

namespace TodoApp.DependencyServices
{
    public static class ValidationInjection
    {
        public static IServiceCollection AddValidationServices(this IServiceCollection services)
        {
            services.AddTransient<IValidator<LoginDto>, LoginModelValidator>();
            services.AddTransient<IValidator<UserCreateDto>, UserCreateValidator>();
            services.AddTransient<IValidator<TodoCreateDto>, TodoCreateValidator>();
            services.AddTransient<IValidator<TodoUpdateDto>, TodoUpdateValidator>();

            services.AddSingleton<IMediator, Mediator>();
            return services;
        }
    }
}
