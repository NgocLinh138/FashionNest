using FashionNest.Application.Common.Messaging;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;
using FashionNest.Domain.Constants;

namespace FashionNest.Application.Features.Coupons.Commands.DeleteCoupon
{
    public class DeleteCouponCommandHandler : ICommandHandler<DeleteCouponCommand, Guid>
    {
        private readonly ICouponRepository couponRepository;

        public DeleteCouponCommandHandler(ICouponRepository couponRepository)
        {
            this.couponRepository = couponRepository;
        }

        public async Task<Result<Guid>> Handle(DeleteCouponCommand request, CancellationToken cancellationToken)
        {
            var coupon = await couponRepository.GetByIdAsync(CouponId.Of(request.Id));
            if (coupon == null)
                return Result.Failure<Guid>(CouponError.NotFound);

            try
            {
                await couponRepository.DeleteAsync(coupon);
                await couponRepository.SaveAsync();

                return Result.Success(coupon.Id.Value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
