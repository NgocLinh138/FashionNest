using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Coupons.Responses;
using FashionNest.Application.Features.Orders.Responses;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;
using FashionNest.Domain.Constants;

namespace FashionNest.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, GetOrderByIdResult>
    {
        private readonly IOrderRepository orderRepository;
        private readonly ICouponRepository couponRepository;

        public GetOrderByIdQueryHandler(IOrderRepository orderRepository, ICouponRepository couponRepository)
        {
            this.orderRepository = orderRepository;
            this.couponRepository = couponRepository;
        }

        public async Task<Result<GetOrderByIdResult>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetByIdAsync(OrderId.Of(request.Id));
            if (order == null)
                return Result.Failure<GetOrderByIdResult>(OrderError.NotFound);

            CouponResponse? couponResponse = null;
            if (order.CouponId != null)
            {
                var coupon = await couponRepository.GetByIdAsync(CouponId.Of(order.CouponId.Value));
                if (coupon != null)
                {
                    couponResponse = new CouponResponse(
                        id: coupon.Id.Value,
                        code: coupon.Code,
                        discountAmount: coupon.DiscountAmount,
                        expirationDate: coupon.ExpirationDate,
                        isActive: coupon.IsActive,
                        description: coupon.Description
                    );
                }
            }

            var response = new GetOrderByIdResult(new OrderResponse(
                id: order.Id.Value,
                userId: order.UserId.Value,
                paymentId: order.PaymentId?.Value,
                status: order.Status,
                orderDate: order.OrderDate,
                couponId: order.CouponId?.Value,
                totalPrice: order.TotalPrice,
                coupon: couponResponse
            ));

            return Result.Success(response);
        }
    }
}
