using FluentValidation;

namespace SNM.Twitter.Application.Features.Commands.Updatetwitter
{
    public class UpdatetwitterCommandValidator : AbstractValidator<UpdatetwitterCommand>
    {
        public UpdatetwitterCommandValidator()
        {
            RuleFor(p => p.Message)
                .NotEmpty().WithMessage("{Message} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{Message} must not exceed 50 characters.");
        }
    }
}