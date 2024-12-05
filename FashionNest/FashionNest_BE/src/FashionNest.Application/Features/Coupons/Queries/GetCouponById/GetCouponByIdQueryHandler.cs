using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Coupons.Responses;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;
using FashionNest.Domain.Constants;

namespace FashionNest.Application.Features.Coupons.Queries.GetCouponById
{
    public class GetCouponByIdQueryHandler : IQueryHandler<GetCouponByIdQuery, GetCouponByIdResult>
    {
        private readonly ICouponRepository couponRepository;
        public GetCouponByIdQueryHandler(ICouponRepository couponRepository)
        {
            this.couponRepository = couponRepository;
        }

        public async Task<Result<GetCouponByIdResult>> Handle(GetCouponByIdQuery request, CancellationToken cancellationToken)
        {
            var coupon = await couponRepository.GetByIdAsync(CouponId.Of(request.Id));

            if (coupon == null)
                return Result.Failure<GetCouponByIdResult>(CouponError.NotFound);

            var response = new GetCouponByIdResult(new CouponResponse(
                coupon.Id.Value,
                coupon.Code,
                coupon.DiscountAmount,
                coupon.ExpirationDate,
                coupon.IsActive,
                coupon.Description));

            return Result.Success(response);
        }
    }
}
