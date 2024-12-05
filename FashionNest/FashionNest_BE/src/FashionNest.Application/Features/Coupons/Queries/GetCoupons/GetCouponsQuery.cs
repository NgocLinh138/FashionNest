using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Coupons.Responses;

namespace FashionNest.Application.Features.Coupons.Queries.GetCoupons
{
    public record GetCouponsQuery(
        int PageIndex,
        int PageSize)
        : IQuery<GetCouponsResult>;

    public record GetCouponsResult(PaginatedResult<CouponResponse> Coupons);
}
