using FashionNest.Domain.Common;

namespace FashionNest.Domain.Constants
{
    public static class PaymentError
    {
        public static Error NotFound = new(
            "Payment.NotFound",
            "Payment not found!");

        public static Error PaymentNotPending = new(
            "Order.PaymentNotPending",
            "The payment associated with this order is not in a pending state.");
    }
}
