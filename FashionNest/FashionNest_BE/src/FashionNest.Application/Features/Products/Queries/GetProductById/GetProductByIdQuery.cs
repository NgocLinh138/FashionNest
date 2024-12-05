using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Products.Responses;

namespace FashionNest.Application.Features.Products.Queries.GetProductById
{
    public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;

    public record GetProductByIdResult(ProductResponse Product);
}
