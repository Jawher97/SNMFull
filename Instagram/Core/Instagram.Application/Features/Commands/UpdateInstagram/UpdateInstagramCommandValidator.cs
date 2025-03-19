using FluentValidation;

namespace SNM.Instagram.Application.Features.Commands.UpdateInstagram
{
    public class UpdateInstagramCommandValidator : AbstractValidator<UpdateInstagramCommand>
    {
        public UpdateInstagramCommandValidator()
        {
            RuleFor(p => p.Message)
                .NotEmpty().WithMessage("{Message} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{Message} must not exceed 50 characters.");
        }
    }
}