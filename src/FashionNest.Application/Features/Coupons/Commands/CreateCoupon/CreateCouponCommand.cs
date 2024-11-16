using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Coupons.Responses;

namespace FashionNest.Application.Features.Coupons.Commands.CreateCoupon
{
    public record CreateCouponCommand(
         string Code,
         decimal DiscountAmount,
         DateTime ExpirationDate,
         bool IsActive,
         string? Description,
         decimal? MinOrderAmount = null,
         int? MaxUses = null,
         int? MaxUsesPerUser = null,
         bool IsNewCustomerOnly = false) : ICommand<CreateCouponResult>;

    public record CreateCouponResult(CouponResponse Coupon);
}
