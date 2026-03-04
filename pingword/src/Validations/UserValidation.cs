using FluentValidation;
using pingword.src.DTOs.Users;

namespace pingword.src.Validations
{
    public class UserValidation : AbstractValidator<UserRegisterRequestDto>
    {
        public UserValidation()
        {
            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }
    }
}
