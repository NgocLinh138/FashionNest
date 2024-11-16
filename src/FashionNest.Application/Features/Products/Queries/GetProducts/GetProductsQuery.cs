using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Products.Responses;

namespace FashionNest.Application.Features.Products.Queries.GetProducts
{
    public record GetProductsQuery(
        int PageIndex,
        int PageSize)
        : IQuery<GetProductsResult>;

    public record GetProductsResult(PaginatedResult<ProductResponse> Products);

}
