using FashionNest.Application.Common.Messaging;

namespace FashionNest.Application.Features.Auth.Commands.ChangePassword
{
    public record ChangePasswordCommand(
        string Email,
        string OldPassword,
        string NewPassword,
        string ConfirmPassword) : ICommand<ChangePasswordResult>;

    public record ChangePasswordResult(ChangePasswordResponse Response);

    public record ChangePasswordResponse(
        bool Flag,
        string Message);
}
