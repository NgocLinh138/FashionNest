using FashionNest.Application.Features.Coupons.Commands.UpdateCoupon;
using FluentValidation;

namespace FashionNest.Application.Features.Coupons.Validators
{
    public class UpdateCouponCommandValidator : AbstractValidator<UpdateCouponCommand>
    {
        public UpdateCouponCommandValidator()
        {
            RuleFor(x => x.DiscountAmount)
                .GreaterThan(0).WithMessage("DiscountAmount must be greater than 0.");
        }
    }
}
