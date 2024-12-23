﻿using System.Net;

namespace FashionNest.Domain.Common.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string message, IEnumerable<Error>? errors = default, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : base(message)
        {
            Errors = errors;
            StatusCode = statusCode;
        }

        public IEnumerable<Error>? Errors { get; }
        public HttpStatusCode StatusCode { get; }
    }
}
