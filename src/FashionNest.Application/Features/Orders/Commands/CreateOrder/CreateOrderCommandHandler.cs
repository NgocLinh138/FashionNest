using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Coupons.Responses;
using FashionNest.Application.Features.Orders.Responses;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;
using FashionNest.Domain.Constants;
using FashionNest.Domain.Enums;

namespace FashionNest.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, CreateOrderResult>
    {
        private readonly IOrderRepository orderRepository;
        private readonly ICouponRepository couponRepository;
        private readonly IUserRepository userRepository;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, ICouponRepository couponRepository, IUserRepository userRepository)
        {
            this.orderRepository = orderRepository;
            this.couponRepository = couponRepository;
            this.userRepository = userRepository;
        }

        public async Task<Result<CreateOrderResult>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            if (request.OrderItems == null || !request.OrderItems.Any())
                return Result.Failure<CreateOrderResult>(OrderError.InvalidOrderItems);

            try
            {
                var order = new Order(
                    userId: UserId.Of(request.UserId),
                    status: OrderStatus.Draft,
                    orderDate: DateTime.UtcNow,
                    couponId: request.CouponId.HasValue ? CouponId.Of(request.CouponId.Value) : null);

                foreach (var item in request.OrderItems)
                {
                    order.AddOrderItem(
                        productId: ProductId.Of(item.ProductId),
                        quantity: item.Quantity,
                        price: item.Price
                    );
                }

                CouponResponse? couponResponse = null;
                if (request.CouponId.HasValue)
                {
                    var coupon = await couponRepository.GetByIdAsync(CouponId.Of(request.CouponId.Value));
                    if (coupon == null || !coupon.IsValid(order, await orderRepository.GetOrdersByUserIdAsync(request.UserId)))
                    {
                        return Result.Failure<CreateOrderResult>(OrderError.InvalidCoupon);
                    }

                    coupon.ApplyToOrder(order);
                    await couponRepository.UpdateAsync(coupon);
                    await couponRepository.SaveAsync();

                    couponResponse = new CouponResponse(
                        coupon.Id.Value,
                        coupon.Code,
                        coupon.DiscountAmount,
                        coupon.ExpirationDate,
                        coupon.IsActive,
                        coupon.Description
                    );
                }

                await orderRepository.InsertAsync(order);
                await orderRepository.SaveAsync();

                var response = new CreateOrderResult(new OrderResponse(
                    order.Id.Value,
                    order.UserId.Value,
                    order.PaymentId?.Value,
                    order.Status,
                    order.OrderDate,
                    order.CouponId?.Value,
                    order.TotalPrice,
                    couponResponse
                ));

                return Result.Success(response);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
