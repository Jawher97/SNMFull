using FluentValidation;

namespace SNM.Twitter.Application.Features.Commands.Createtwitter
{
    public class CreatetwitterCommandValidator : AbstractValidator<CreatetwitterCommand>
    {
        public CreatetwitterCommandValidator()
        {
            RuleFor(p => p.Message)
                .NotEmpty().WithMessage("{Message} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{Message} must not exceed 50 characters.");
        }
    }
}