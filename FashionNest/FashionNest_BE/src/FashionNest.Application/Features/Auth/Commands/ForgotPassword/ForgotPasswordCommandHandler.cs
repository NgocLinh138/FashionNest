using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Services.Interface;
using FashionNest.Domain.Common.Repositories;
using FashionNest.Domain.Common;
using FashionNest.Domain.Constants;
using Microsoft.Extensions.Caching.Memory;

namespace FashionNest.Application.Features.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordCommandHandler : ICommandHandler<ForgotPasswordCommand, ForgotPasswordResult>
    {
        private readonly IUserRepository userRepository;
        private readonly IEmailService emailService;
        private readonly IJWTTokenService jWTTokenService;
        private readonly IMemoryCache memoryCache;

        public ForgotPasswordCommandHandler(IUserRepository userRepository, IEmailService emailService, IJWTTokenService jWTTokenService, IMemoryCache memoryCache)
        { 
            this.userRepository = userRepository;
            this.emailService = emailService;
            this.jWTTokenService = jWTTokenService;
            this.memoryCache = memoryCache;
        }

        public async Task<Result<ForgotPasswordResult>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.FindUserByEmail(request.Email);
            if (user == null)
                return Result.Failure<ForgotPasswordResult>(UserError.NotFound);

            var token = jWTTokenService.GenerateRefreshToken();

            var cacheKey = $"ResetPasswordToken_{user.Email}";
            var expirationTime = DateTime.UtcNow.AddHours(1);
            memoryCache.Set(cacheKey, token, expirationTime);

            var body = $"Bạn đã yêu cầu reset mật khẩu. Vui lòng sử dụng mã token dưới đây để reset mật khẩu: <b>{token}</b>.";
            await emailService.SendEmailAsync(request.Email, "Mã Reset Mật Khẩu", body);

            return Result.Success(new ForgotPasswordResult(new ForgotPasswordResponse(true, "Reset password email sent.")));
        }
    }
}
