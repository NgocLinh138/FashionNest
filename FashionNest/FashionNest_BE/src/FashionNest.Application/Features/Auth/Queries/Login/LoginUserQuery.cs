using FashionNest.Application.Common.Messaging;

namespace FashionNest.Application.Features.Auth.Queries.Login
{
    public record LoginUserQuery(string? Email, string? Password) : IQuery<LoginResult>;

    public record LoginResult(LoginResponse User);

    public record LoginResponse(
        bool Flag,
        string? Message = null,
        string? Role = null,
        string? Token = null);
}
