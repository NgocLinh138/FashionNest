using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Orders.Responses;

namespace FashionNest.Application.Features.Orders.Queries.GetOrders
{
    public record GetOrdersQuery(
        Guid? UserId = null,
        int PageIndex = 1,
        int PageSize = 10)
        : IQuery<GetOrdersResult>;

    public record GetOrdersResult(PaginatedResult<OrderResponse> Orders);
}
