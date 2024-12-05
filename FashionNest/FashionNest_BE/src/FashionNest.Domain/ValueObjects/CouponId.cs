using FashionNest.Domain.Common.Exceptions;

namespace FashionNest.Domain.ValueObjects
{
    public record CouponId
    {
        public Guid Value { get; }
        private CouponId(Guid value) => Value = value;

        public static CouponId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value == Guid.Empty)
            {
                throw new DomainException("CouponId cannot be null.");
            }
            return new CouponId(value);
        }
    }
}
