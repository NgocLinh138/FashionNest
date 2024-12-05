using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Products.Responses;

namespace FashionNest.Application.Features.Products.Queries.GetProductByCategory
{
    public record GetProductByCategoryQuery(string CategoryName) : IQuery<GetProductByCategoryResult>;

    public record GetProductByCategoryResult(IEnumerable<ProductResponse> Products);
}
