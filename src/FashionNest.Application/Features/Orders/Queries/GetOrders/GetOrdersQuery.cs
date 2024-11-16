using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Orders.Responses;

namespace FashionNest.Application.Features.Orders.Queries.GetOrders
{
    public record GetOrdersQuery(
        int PageIndex,
        int PageSize)
        : IQuery<GetOrdersResult>;

    public record GetOrdersResult(PaginatedResult<OrderResponse> Orders);
}
