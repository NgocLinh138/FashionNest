using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Categories.Responses;

namespace FashionNest.Application.Features.Categories.Queries.GetCategories
{
    public record GetCategoriesQuery(
        string? Filter = null,
        string? SortBy = null,
        bool SortOrderAscending = true,
        int PageIndex = 1,
        int PageSize = 10)
        : IQuery<GetCategoriesResult>;

    public record GetCategoriesResult(PaginatedResult<CategoryResponse> Categories);
}
