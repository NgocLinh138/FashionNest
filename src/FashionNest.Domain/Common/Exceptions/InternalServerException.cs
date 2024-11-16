using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Exceptions;
using System.Net;

namespace FashionNest.Application.Common.Exceptions
{
    public class InternalServerException : DomainException
    {
        public InternalServerException(string message, List<Error>? errors = default) : base(message, errors, HttpStatusCode.InternalServerError)
        {
        }
    }
}
