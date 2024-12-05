using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Coupons.Responses;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;

namespace FashionNest.Application.Features.Coupons.Queries.GetCoupons
{
    public class GetCouponsQueryHandler : IQueryHandler<GetCouponsQuery, GetCouponsResult>
    {
        private readonly ICouponRepository couponRepository;

        public GetCouponsQueryHandler(ICouponRepository couponRepository)
        {
            this.couponRepository = couponRepository;
        }

        public async Task<Result<GetCouponsResult>> Handle(GetCouponsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var coupons = await couponRepository.GetAsync(
                    orderByAsc: true,
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize);

                var response = coupons.Select(coupon => new CouponResponse(
                    coupon.Id.Value,
                    coupon.Code,
                    coupon.DiscountAmount,
                    coupon.ExpirationDate,
                    coupon.IsActive,
                    coupon.Description)).ToList();

                return Result.Success(new GetCouponsResult(new PaginatedResult<CouponResponse>(response, response.Count, request.PageIndex, request.PageSize)));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

}
