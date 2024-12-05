using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Products.Responses;
using Microsoft.AspNetCore.Http;

namespace FashionNest.Application.Features.Products.Commands.CreateProduct
{
    public record CreateProductCommand(
        string Name,
        string? Description,
        decimal Price,
        int Stock,
        IFormFile? Image,
        bool IsRental,
        Guid CategoryId) : ICommand<CreateProductResult>;

    public record CreateProductResult(ProductResponse Product);
}
