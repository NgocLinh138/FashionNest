using FashionNest.Application.Features.Coupons.Responses;
using FashionNest.Domain.Enums;

namespace FashionNest.Application.Features.Orders.Responses
{
    public class OrderResponse
    {
        public OrderResponse(Guid id, Guid userId, Guid? paymentId, OrderStatus status, DateTime orderDate, Guid? couponId, decimal totalPrice, CouponResponse? coupon = null)
        {
            Id = id;
            UserId = userId;
            PaymentId = paymentId;
            Status = status;
            OrderDate = orderDate.ToString("dd/MM/yyyy"); 
            CouponId = couponId;
            TotalPrice = totalPrice;
            Coupon = coupon; 
        }

        public Guid Id { get; }
        public Guid UserId { get; }
        public Guid? PaymentId { get; }
        public OrderStatus Status { get; }
        public string OrderDate { get; }
        public Guid? CouponId { get; }
        public decimal TotalPrice { get; }
        public CouponResponse? Coupon { get; } 
    }
}
