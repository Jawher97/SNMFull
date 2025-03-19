using FluentValidation;

namespace SNM.Instagram.Application.Features.Commands.CreateInstagram
{
    public class CreateInstagramCommandValidator : AbstractValidator<CreateInstagramCommand>
    {
        public CreateInstagramCommandValidator()
        {
            RuleFor(p => p.Message)
                .NotEmpty().WithMessage("{Message} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{Message} must not exceed 50 characters.");
        }
    }
}