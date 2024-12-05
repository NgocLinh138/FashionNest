using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Users.Responses;

namespace FashionNest.Application.Features.Users.Queries.GetUsers
{
    public record GetUsersQuery(
        string? Filter = null,          
        string? SortBy = null,          
        bool SortOrderAscending = true,
        bool? IsActive = null,
        int PageIndex = 1,
        int PageSize = 10)
        : IQuery<GetUsersResult>;

    public record GetUsersResult(PaginatedResult<UserGetAllResponse> Users);
}
