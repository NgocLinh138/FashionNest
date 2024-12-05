using FashionNest.Application.Features.Coupons.Commands.CreateCoupon;
using FluentValidation;

namespace FashionNest.Application.Features.Coupons.Validators
{
    public class CreateCouponCommandValidator : AbstractValidator<CreateCouponCommand>
    {
        public CreateCouponCommandValidator()
        {
            RuleFor(x => x.DiscountAmount)
                .GreaterThan(0).WithMessage("DiscountAmount must be greater than 0.");
        }
    }
}
