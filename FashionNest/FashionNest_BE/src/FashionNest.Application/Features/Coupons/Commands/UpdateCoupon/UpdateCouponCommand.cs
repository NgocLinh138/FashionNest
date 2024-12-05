using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Coupons.Responses;

namespace FashionNest.Application.Features.Coupons.Commands.UpdateCoupon
{
    public record UpdateCouponCommand(
        Guid Id,
        string Code,
        decimal DiscountAmount,
        DateTime ExpirationDate,
        bool IsActive,
        string? Description) : ICommand<UpdateCouponResult>;

    public record UpdateCouponResult(CouponResponse Coupon);
}
