using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Categories.Responses;

namespace FashionNest.Application.Features.Categories.Queries.GetCategoryById
{
    public record GetCategoryByIdQuery(Guid Id) : IQuery<GetCategoryByIdResult>;
    public record GetCategoryByIdResult(CategoryResponse Category);
}
