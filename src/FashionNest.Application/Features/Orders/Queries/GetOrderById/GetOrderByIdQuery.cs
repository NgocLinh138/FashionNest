using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Orders.Responses;

namespace FashionNest.Application.Features.Orders.Queries.GetOrderById
{
    public record GetOrderByIdQuery(Guid Id) : IQuery<GetOrderByIdResult>;

    public record GetOrderByIdResult(OrderResponse Order);
}
