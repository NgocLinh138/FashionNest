using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Categories.Responses;

namespace FashionNest.Application.Features.Categories.Queries.GetCategories
{
    public record GetCategoriesQuery(
        int PageIndex,
        int PageSize)
        : IQuery<GetCategoriesResult>;

    public record GetCategoriesResult(PaginatedResult<CategoryResponse> Categories);
}
