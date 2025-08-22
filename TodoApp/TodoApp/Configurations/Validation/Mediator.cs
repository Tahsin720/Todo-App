// Ignore Spelling: validators
using FluentValidation;
using FluentValidation.Results;

namespace TodoApp.Configurations.Validation
{
    public interface IMediator
    {
        Task<ValidationResult> ValidateAsync<T>(T obj);
    }
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public Mediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<ValidationResult> ValidateAsync<T>(T obj)
        {
            var validator = _serviceProvider.GetService<IValidator<T>>();

            if (validator is null)
            {
                return new ValidationResult(new[] { new ValidationFailure(string.Empty, "No validator found.") });
            }

            return await validator.ValidateAsync(obj);
        }
    }
}
