using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Roles.Responses;

namespace FashionNest.Application.Features.Roles.Queries.GetRoleById
{
    public record GetRoleByIdQuery(Guid Id) : IQuery<GetRoleResult>;

    public record GetRoleResult(RoleResponse Role);
}
