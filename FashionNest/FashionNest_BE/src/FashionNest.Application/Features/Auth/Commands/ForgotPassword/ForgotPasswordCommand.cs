using FashionNest.Application.Common.Messaging;

namespace FashionNest.Application.Features.Auth.Commands.ForgotPassword
{
    public record ForgotPasswordCommand(string Email) : ICommand<ForgotPasswordResult>;

    public record ForgotPasswordResult(ForgotPasswordResponse Response);

    public record ForgotPasswordResponse(
        bool Flag,
        string Message);
}
