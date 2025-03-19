using FluentValidation;

namespace SNM.Instagram.Application.Features.Commands.Brands.UpdateBrand
{
    public class UpdateBrandCommandValidator : AbstractValidator<UpdateBrandCommand>
    {
        public UpdateBrandCommandValidator()
        {
            RuleFor(p => p.DisplayName)
                .NotEmpty().WithMessage("{DisplayName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{DisplayName} must not exceed 50 characters.");
        }
    }
}
