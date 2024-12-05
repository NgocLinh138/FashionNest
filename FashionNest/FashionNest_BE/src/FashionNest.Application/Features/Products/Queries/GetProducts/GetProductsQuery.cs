using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Products.Responses;

namespace FashionNest.Application.Features.Products.Queries.GetProducts
{
    public record GetProductsQuery(
        string? Filter = null,
        string? SortBy = null,
        bool SortOrderAscending = true,
        int PageIndex = 1,
        int PageSize = 10)
    : IQuery<GetProductsResult>;

    public record GetProductsResult(PaginatedResult<ProductResponse> Products);

}
