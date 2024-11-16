using FashionNest.Domain.Common.Exceptions;
using System.Net;

namespace FashionNest.Application.Common.Exceptions
{
    public class NotFoundException : DomainException
    {
        public NotFoundException(string message) : base(message, null, HttpStatusCode.NotFound)
        {
        }
    }
}
