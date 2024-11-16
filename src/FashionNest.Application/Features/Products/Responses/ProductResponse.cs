using FashionNest.Application.Features.Categories.Responses;

namespace FashionNest.Application.Features.Products.Responses
{
    public record ProductResponse(
        Guid Id,
        string Name,
        string? Description,
        decimal Price,
        int Stock,
        string? Image,
        bool IsRental,
        CategoryResponse Category);

    public record DeleteProductResponse(
        Guid Id,
        string Name,
        string? Description,
        decimal Price,
        int Stock,
        string? Image,
        bool IsRental,
        Guid CategoryId);
}
