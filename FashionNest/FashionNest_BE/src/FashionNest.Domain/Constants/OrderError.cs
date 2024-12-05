using FashionNest.Domain.Common;

namespace FashionNest.Domain.Constants
{
    public static class OrderError
    {
        public static Error NotFound = new(
            "Order.NotFound",
            "Order not found!");

        public static Error InvalidOrderItems = new(
          "Order.InvalidOrderItems",
          "Order must contain at least one item.");

        public static Error InvalidCoupon = new(
          "Order.InvalidCoupon",
          "Invalid Coupon.");


        public static Error CouponUsageLimitExceeded = new(
          "Order.CouponUsageLimitExceeded",
          "CouponUsageLimitExceeded.");
    }
}

