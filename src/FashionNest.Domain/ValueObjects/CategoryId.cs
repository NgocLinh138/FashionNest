using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Exceptions;
using System.Net;

namespace FashionNest.Domain.ValueObjects
{
    public record CategoryId
    {
        public Guid Value { get; }
        private CategoryId(Guid value) => Value = value;

        public static CategoryId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value == Guid.Empty)
            {
                throw new DomainException("CategoryId cannot be null.", Enumerable.Empty<Error>(), HttpStatusCode.BadRequest);
            }

            return new CategoryId(value);
        }
    }
}
