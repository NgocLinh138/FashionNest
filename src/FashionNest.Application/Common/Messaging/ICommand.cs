using FashionNest.Domain.Common;
using MediatR;

namespace FashionNest.Application.Common.Messaging
{
    public interface ICommand : IRequest<Result>
    {
    }

    public interface ICommand<TResponse> : IRequest<Result<TResponse>>
    {
    }
}
