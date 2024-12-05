using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Coupons.Responses;
using FashionNest.Application.Features.Orders.Responses;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;
using FashionNest.Domain.Constants;
using FashionNest.Domain.Enums;
using FashionNest.Infrastructure.VnPay.Repository.Interface;

namespace FashionNest.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, CreateOrderResult>
    {
        private readonly IOrderRepository orderRepository;
        private readonly ICouponRepository couponRepository;
        private readonly IPaymentRepository paymentRepository;
        private readonly IVnPayRepository vnPayRepository;

        public CreateOrderCommandHandler(
            IOrderRepository orderRepository, 
            ICouponRepository couponRepository, 
            IPaymentRepository paymentRepository, 
            IVnPayRepository vnPayRepository)
        {
            this.orderRepository = orderRepository;
            this.couponRepository = couponRepository;
            this.paymentRepository = paymentRepository;
            this.vnPayRepository = vnPayRepository;
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

                Payment payment = null;
                string? paymentUrl = null;
                if (request.PaymentMethod == PaymentMethod.VNPay)
                {
                    // Handle VNPay payment
                    payment = new Payment(
                        UserId.Of(request.UserId),
                        PaymentMethod.VNPay,
                        order.TotalPrice,
                        "Payment for order via VNPay",
                        DateTime.UtcNow,
                        PaymentStatus.Pending
                    );

                    await paymentRepository.InsertAsync(payment);
                    await paymentRepository.SaveAsync();

                    paymentUrl = vnPayRepository.CreatePaymentUrl(
                        order.TotalPrice,
                        payment.Description,
                        payment.Id.Value.ToString(),
                        "127.0.0.1" 
                    );

                    if (paymentUrl == null)
                        return Result.Failure<CreateOrderResult>(Error.PaymentUrlGenerationFailed);

                    order.PaymentId = payment.Id;
                }
                else if (request.PaymentMethod == PaymentMethod.COD)
                {
                    // Handle COD 
                    payment = new Payment(
                        UserId.Of(request.UserId),
                        PaymentMethod.COD,
                        order.TotalPrice,
                        "Payment for order via Cash on Delivery",
                        DateTime.UtcNow,
                        PaymentStatus.Pending
                    );

                    await paymentRepository.InsertAsync(payment);
                    await paymentRepository.SaveAsync();

                    order.PaymentId = payment.Id;
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

                var paymentMethod = payment?.Method;

                var response = new CreateOrderResult(new CreateOrderResponse(
                    order.Id.Value,
                    order.UserId.Value,
                    order.PaymentId?.Value,
                    paymentMethod,
                    payment?.Status ?? PaymentStatus.Pending, 
                    order.Status,
                    order.OrderDate,
                    order.CouponId?.Value,
                    order.TotalPrice,
                    paymentUrl, 
                    couponResponse));

                return Result.Success(response);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
