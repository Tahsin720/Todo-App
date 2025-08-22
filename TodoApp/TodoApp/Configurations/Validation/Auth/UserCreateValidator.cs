// Ignore Spelling: Validator Auth HRM Api

using FluentValidation;
using TodoApp.Models.Auth;

namespace TodoApp.Configurations.Validation.Auth
{
    public class UserCreateValidator : AbstractValidator<UserCreateDto>
    {
        public UserCreateValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .NotNull()
                .WithMessage("Email cannot be null.")
                .EmailAddress()
                .WithMessage("Email must be a valid email address.");

            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("Full name is required.")
                .NotNull()
                .WithMessage("Full name cannot be null.")
                .MinimumLength(3)
                .WithMessage("Full name must be at least 3 characters long.")
                .MaximumLength(100)
                .WithMessage("Full name cannot exceed 100 characters.");
        }
    }
}
