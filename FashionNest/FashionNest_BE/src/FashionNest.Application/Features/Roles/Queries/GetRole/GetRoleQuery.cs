using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Roles.Responses;

namespace FashionNest.Application.Features.Roles.Queries.GetRole
{
    public record GetRolesQuery(
        int pageIndex,
        int pageSize) 
        : IQuery<GetRolesResult>;

    public record GetRolesResult(PaginatedResult<RoleResponse> Roles);
}
