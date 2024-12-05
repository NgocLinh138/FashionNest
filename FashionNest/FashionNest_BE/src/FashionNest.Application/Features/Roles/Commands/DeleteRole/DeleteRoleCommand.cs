using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Roles.Responses;

namespace FashionNest.Application.Features.Roles.Commands.DeleteRole
{
    public record DeleteRoleCommand(Guid Id) : ICommand<Guid>;
}
