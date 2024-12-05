using FashionNest.Domain.Common;
using MediatR;

namespace FashionNest.Application.Common.Messaging
{
    public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
        where TQuery : IQuery<TResponse>
    {
    }
}
