using FashionNest.Domain.Common.Exceptions;

namespace FashionNest.Domain.ValueObjects
{
    public record UserId
    {
        public Guid Value { get; }
        public UserId(Guid value) => Value = value;

        public static UserId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value == Guid.Empty)
            {
                throw new DomainException("UserId cannot be null.");
            }
            return new UserId(value);
        }
    }
}
