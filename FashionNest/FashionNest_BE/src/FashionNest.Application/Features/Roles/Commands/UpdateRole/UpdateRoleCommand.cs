using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Roles.Responses;

namespace FashionNest.Application.Features.Roles.Commands.UpdateRole
{
    public record UpdateRoleCommand(
        Guid Id,
        string Name,
        string Description) : ICommand<UpdateRoleResult>;
    public record UpdateRoleResult(RoleResponse Role);
}
