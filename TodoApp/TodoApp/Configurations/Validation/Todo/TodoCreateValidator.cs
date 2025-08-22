using FluentValidation;
using TodoApp.Models.Todo;

namespace TodoApp.Configurations.Validation.Todo
{
    public class TodoCreateValidator : AbstractValidator<TodoCreateDto>
    {
        public TodoCreateValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .NotNull().WithMessage("Title cannot be null.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .NotNull().WithMessage("Description cannot be null.")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
        }
    }
}
