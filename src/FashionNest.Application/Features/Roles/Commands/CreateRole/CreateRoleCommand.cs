using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Roles.Responses;

namespace FashionNest.Application.Features.Roles.Commands.CreateRole
{
    public record CreateRoleCommand
        (string Name,
        string Description) 
        : ICommand<CreateRoleResult>;

    public record CreateRoleResult(RoleResponse Role);
}
