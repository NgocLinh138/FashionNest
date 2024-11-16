using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Products.Responses;

namespace FashionNest.Application.Features.Products.Commands.UpdateProduct
{
    public record UpdateProductCommand(
        Guid Id,
        string Name,
        string? Description,
        decimal Price,
        int Stock,
        string? Image,
        bool IsRental,
        Guid CategoryId) : ICommand<UpdateProductResult>;

    public record UpdateProductResult(ProductResponse Product);
}
