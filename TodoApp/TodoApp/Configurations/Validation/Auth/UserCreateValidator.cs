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

            //RuleFor(x => x.Password)
            //    .NotEmpty()
            //    .WithMessage("Password is required.")
            //    .NotNull()
            //    .WithMessage("Password cannot be null.")
            //    .MinimumLength(8)
            //    .WithMessage("Password must be at least 8 characters long.")
            //    .Matches(@"[A-Z]")
            //    .WithMessage("Password must contain at least one uppercase letter.")
            //    .Matches(@"[a-z]")
            //    .WithMessage("Password must contain at least one lowercase letter.")
            //    .Matches(@"\d")
            //    .WithMessage("Password must contain at least one digit.")
            //    .Matches(@"[\!\@\#\$\%\^\&\*\(\)\-\+\=]")
            //    .WithMessage("Password must contain at least one special character (!@#$%^&*()-+=).");
        }
    }
}
