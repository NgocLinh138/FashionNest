using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Services.Interface;
using FashionNest.Domain.Common.Repositories;
using FashionNest.Domain.Common;
using FashionNest.Domain.Constants;
using Microsoft.Extensions.Caching.Memory;

namespace FashionNest.Application.Features.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, ResetPasswordResult>
    {
        private readonly IUserRepository userRepository;
        private readonly IJWTTokenService jWTTokenService;
        private readonly IMemoryCache memoryCache;

        public ResetPasswordCommandHandler(IUserRepository userRepository, IJWTTokenService jWTTokenService, IMemoryCache memoryCache)
        {
            this.userRepository = userRepository;
            this.jWTTokenService = jWTTokenService;
            this.memoryCache = memoryCache;
        }

        public async Task<Result<ResetPasswordResult>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            // Xác thực token
            var cacheKey = $"ResetPasswordToken_{request.Email}";
            if (!memoryCache.TryGetValue(cacheKey, out string storedToken))
                return Result.Failure<ResetPasswordResult>(UserError.InvalidToken);

            if (storedToken != request.Token)
                return Result.Failure<ResetPasswordResult>(UserError.InvalidToken);

            var user = await userRepository.FindUserByEmail(request.Email);
            if (user == null)
                return Result.Failure<ResetPasswordResult>(UserError.NotFound);

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            user.Password = hashedPassword;
            await userRepository.SaveAsync();

            memoryCache.Remove(cacheKey);

            return Result.Success(new ResetPasswordResult(new ResetPasswordResponse(true, "Password has been successfully updated.")));
        }
    }
}
