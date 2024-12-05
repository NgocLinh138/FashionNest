using FashionNest.Application.Common.Exceptions;
using FashionNest.Domain.Common.Exceptions;

namespace FashionNest.Domain.ValueObjects
{
    public record ProductId
    {
        public Guid Value { get; }
        private ProductId(Guid value) => Value = value;

        public static ProductId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value == Guid.Empty)
            {
                throw new DomainException("ProductId cannot be null.");
            }
            return new ProductId(value);
        }
    }
}
