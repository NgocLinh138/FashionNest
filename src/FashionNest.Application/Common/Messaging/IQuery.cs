using FashionNest.Domain.Common;
using MediatR;

namespace FashionNest.Application.Common.Messaging
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {
    }
}
