using FashionNest.Domain.Common;

namespace FashionNest.Domain.Constants
{
    public static class CouponError
    {
        public static Error NotFound = new(
            "Coupon.NotFound",
            "Coupon not found!");
    }
}
