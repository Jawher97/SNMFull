using FluentValidation;

namespace SNM.Instagram.Application.Features.Commands.Brands.CreateBrand
{
    public class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
    {
        public CreateBrandCommandValidator()
        {
            RuleFor(p => p.Brand.DisplayName)
                .NotEmpty().WithMessage("{DisplayName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{DisplayName} must not exceed 50 characters.");
        }
    }
}