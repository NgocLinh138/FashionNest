using FashionNest.Application.Common.Messaging;

namespace FashionNest.Application.Features.Auth.Commands.Register
{
    public record RegisterUserCommand(
        string Name,
        string Email,
        string Password,
        string ConfirmPassword,
        string PhoneNumber,
        string Address)
        : ICommand<RegisterUserResult>;

    public record RegisterUserResult(RegisterUserResponse User);

    public record RegisterUserResponse(
        bool Flag,
        string Message = null!);
}
