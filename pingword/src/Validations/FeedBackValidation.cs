using FluentValidation;
using pingword.src.DTOs.FeedBacks;

namespace pingword.src.Validations
{
    public class FeedBackValidation : AbstractValidator<FeedBackRequestDto>
    {
        public FeedBackValidation()
        {
            RuleFor(f => f.User)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
            RuleFor(f => f.Message)
                .NotEmpty().WithMessage("Message is required.")
                .MaximumLength(1000).WithMessage("Message cannot exceed 1000 characters.");
        }
    }
}
