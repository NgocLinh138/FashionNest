using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Coupons.Responses;

namespace FashionNest.Application.Features.Coupons.Queries.GetCouponById
{
    public record GetCouponByIdQuery(Guid Id) : IQuery<GetCouponByIdResult>;

    public record GetCouponByIdResult(CouponResponse Coupon);
}
