using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Coupons.Responses;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;
using FashionNest.Domain.Constants;

namespace FashionNest.Application.Features.Coupons.Commands.UpdateCoupon
{
    public class UpdateCouponCommandHandler : ICommandHandler<UpdateCouponCommand, UpdateCouponResult>
    {
        private readonly ICouponRepository couponRepository;

        public UpdateCouponCommandHandler(ICouponRepository couponRepository)
        {
            this.couponRepository = couponRepository;
        }

        public async Task<Result<UpdateCouponResult>> Handle(UpdateCouponCommand request, CancellationToken cancellationToken)
        {
            var coupon = await couponRepository.GetByIdAsync(CouponId.Of(request.Id));
            if (coupon == null)
                return Result.Failure<UpdateCouponResult>(CouponError.NotFound);

            try
            {
                coupon.Update(request.Code, request.DiscountAmount, request.ExpirationDate, request.IsActive, request.Description);
                await couponRepository.UpdateAsync(coupon);
                await couponRepository.SaveAsync();

                return Result.Success(new UpdateCouponResult(
                    new CouponResponse(
                        coupon.Id.Value,
                        coupon.Code,
                        coupon.DiscountAmount,
                        coupon.ExpirationDate,
                        coupon.IsActive,
                        coupon.Description)));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
