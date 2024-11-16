using FashionNest.Application.Services.Interface;
using FashionNest.Application.Services.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace FashionNest.Application.Services
{
    public class JWTTokenService : IJWTTokenService
    {
        private readonly JWTOption jwtOption = new JWTOption();

        public JWTTokenService(IConfiguration configuration)
        {
            jwtOption = new JWTOption
            {
                SecretKey = configuration["JWTOption:SecretKey"],
                Audience = configuration["JWTOption:Audience"],
                Issuer = configuration["JWTOption:Issuer"],
                ExpireMin = int.Parse(configuration["JWTOption:ExpireMin"])
            };
        }

        public string GenerateAccessToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, UserId.Of(user.Id.Value).ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var token = new JwtSecurityToken
            (
                issuer: jwtOption.Issuer,
                audience: jwtOption.Audience,
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(jwtOption.ExpireMin),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var key = Encoding.UTF8.GetBytes(jwtOption.SecretKey);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false, 
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),   
                ClockSkew = TimeSpan.Zero //giảm tgian chênh lệch đồng hồ
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            var jwtSecurityToken = (JwtSecurityToken)securityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCulture))
                throw new SecurityTokenException("Invalid Token");

            return principal;
        }
    }
}
