using FashionNest.Application.Common.Exceptions;
using FashionNest.Domain.Common.Exceptions;

namespace FashionNest.Domain.ValueObjects
{
    public class RoleId
    {
        public Guid Value { get; }
        public RoleId(Guid value) => Value = value;

        public override string ToString()
        {
            return Value.ToString(); 
        }

        public static RoleId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value == Guid.Empty)
            {
                throw new DomainException("RoleId cannot be null.");
            }
            return new RoleId(value);
        }
    }
}
