using FashionNest.Application.Features.Coupons.Responses;
using FashionNest.Domain.Enums;

namespace FashionNest.Application.Features.Orders.Responses
{
    public class CreateOrderResponse
    {
        public CreateOrderResponse(Guid id, Guid userId, Guid? paymentId, PaymentMethod? paymentMethod, PaymentStatus? paymentStatus, OrderStatus status, DateTime orderDate, Guid? couponId, decimal totalPrice, string? paymentUrl, CouponResponse? coupon = null)
        {
            Id = id;
            UserId = userId;
            PaymentId = paymentId;
            PaymentMethod = paymentMethod;
            PaymentStatus = paymentStatus;
            Status = status;
            OrderDate = orderDate.ToString("dd/MM/yyyy");
            CouponId = couponId;
            TotalPrice = totalPrice;
            PaymentUrl = paymentUrl;
            Coupon = coupon;
        }

        public Guid Id { get; }
        public Guid UserId { get; }
        public Guid? PaymentId { get; }
        public PaymentMethod? PaymentMethod { get; }
        public PaymentStatus? PaymentStatus { get; }
        public OrderStatus Status { get; }
        public string OrderDate { get; }
        public Guid? CouponId { get; }
        public decimal TotalPrice { get; }
        public string? PaymentUrl { get; }
        public CouponResponse? Coupon { get; }
    }
}
