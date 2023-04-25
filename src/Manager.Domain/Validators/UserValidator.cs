using FluentValidation;
using Manager.Domain.Entities;

namespace Manager.Domain.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x)
                .NotEmpty()
                .WithMessage("The Entity cannot be empty")
                .NotNull()
                .WithMessage("The Entity cannot be null");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("The Name cannot be empty")
                .NotNull()
                .WithMessage("The Name cannot be null")
                .MinimumLength(3)
                .WithMessage("The Name must be at least 3 characters long")
                .MaximumLength(80)
                .WithMessage("The Name must have a maximum of 80 characters");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("The Email cannot be empty")
                .NotNull()
                .WithMessage("The Email cannot be null")
                .MinimumLength(10)
                .WithMessage("The Email must be at least 10 characters long")
                .MaximumLength(80)
                .WithMessage("The Email must have a maximum of 80 characters")
                .Matches(@"[^@ \t\r\n]+@[^@ \t\r\n]+\.[^@ \t\r\n]+")
                .WithMessage("The Email is not valid");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("The Password cannot be empty")
                .NotNull()
                .WithMessage("The Password cannot be null")
                .MinimumLength(8)
                .WithMessage("The Password must be at least 8 characters long")
                .MaximumLength(20)
                .WithMessage("The Password must have a maximum of 15 characters")
                .Matches(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*[\d])(?=.*[@#$%&*!-+&*]).{8,20}$")
                .WithMessage("Password must contain at least one uppercase character, number and special character");
        }
    }
}
