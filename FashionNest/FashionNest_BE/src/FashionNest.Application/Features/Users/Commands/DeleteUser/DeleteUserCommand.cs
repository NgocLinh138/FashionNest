using FashionNest.Application.Common.Messaging;

namespace FashionNest.Application.Features.Users.Commands.UpdateUserStatus
{
    public record DeleteUserCommand(Guid UserId) : ICommand<Guid>;
}
