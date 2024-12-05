using System.Security.Claims;

namespace FashionNest.Application.Services.Interface
{
    public interface IJWTTokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
