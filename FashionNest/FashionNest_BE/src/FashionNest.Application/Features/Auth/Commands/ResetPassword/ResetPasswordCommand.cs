using FashionNest.Application.Common.Messaging;

namespace FashionNest.Application.Features.Auth.Commands.ResetPassword
{
    public record ResetPasswordCommand(string Email, string Token, string NewPassword) : ICommand<ResetPasswordResult>;

    public record ResetPasswordResult(ResetPasswordResponse Response);

    public record ResetPasswordResponse(bool Flag, string Message);
}
