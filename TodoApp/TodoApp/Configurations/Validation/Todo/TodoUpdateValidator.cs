using FluentValidation;
using TodoApp.Models.Todo;

namespace TodoApp.Configurations.Validation.Todo
{
    public class TodoUpdateValidator : AbstractValidator<TodoUpdateDto>
    {
        public TodoUpdateValidator()
        {
            RuleFor(x => x.Title)
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
        }
    }
}
