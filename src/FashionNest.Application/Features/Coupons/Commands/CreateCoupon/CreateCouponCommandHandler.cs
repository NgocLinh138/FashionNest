using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Coupons.Responses;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;

namespace FashionNest.Application.Features.Coupons.Commands.CreateCoupon
{
    public class CreateCouponCommandHandler : ICommandHandler<CreateCouponCommand, CreateCouponResult>
    {
        private readonly ICouponRepository couponRepository;

        public CreateCouponCommandHandler(ICouponRepository couponRepository)
        {
            this.couponRepository = couponRepository;
        }

        public async Task<Result<CreateCouponResult>> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var coupon = new Coupon(
                   code: request.Code,
                   discountAmount: request.DiscountAmount,
                   expirationDate: request.ExpirationDate,
                   isActive: request.IsActive,
                   description: request.Description,
                   minOrderAmount: request.MinOrderAmount,
                   maxUses: request.MaxUses,
                   maxUsesPerUser: request.MaxUsesPerUser,
                   isNewCustomerOnly: request.IsNewCustomerOnly);


                await couponRepository.InsertAsync(coupon);
                await couponRepository.SaveAsync();

                return Result.Success(new CreateCouponResult(
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
