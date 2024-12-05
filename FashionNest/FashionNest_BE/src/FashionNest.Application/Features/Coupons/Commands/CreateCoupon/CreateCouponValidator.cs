using FluentValidation;

namespace FashionNest.Application.Features.Coupons.Commands.CreateCoupon
{
    public class CreateCouponValidator : AbstractValidator<Coupon>
    {
        public CreateCouponValidator()
        {
            RuleFor(x => x.IsActive)
                .Equal(true)
                .WithMessage("Coupon is expired or inactive.");

            RuleFor(x => x.ExpirationDate)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("Coupon has expired.");

            RuleFor(x => x.MinOrderAmount)
                .GreaterThan(0)
                .When(c => c.MinOrderAmount.HasValue)
                .WithMessage("Order amount does not meet the minimum requirement for this coupon.");

            RuleFor(x => x.MaxUses)
                .GreaterThan(0)
                .When(x => x.MaxUses.HasValue)
                .WithMessage("Coupon usage limit must be greater than 0.");

            RuleFor(x => x.MaxUsesPerUser)
                .GreaterThan(0)
                .When(x => x.MaxUsesPerUser.HasValue)
                .WithMessage("Maximum uses per user must be greater than 0.");

            RuleFor(x => x.IsNewCustomerOnly)
                .Must((coupon, isNewCustomerOnly) => !isNewCustomerOnly || coupon.IsNewCustomerOnly)
                .WithMessage("This coupon is only available for new customers.");
        }
    }
}
