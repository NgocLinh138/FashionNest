using FashionNest.Domain.Common.Repositories;
using FashionNest.Domain.Common;
using FashionNest.Domain.Constants;
using FashionNest.Application.Common.Messaging;

namespace FashionNest.Application.Features.Auth.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand, ChangePasswordResult>
    {
        private readonly IUserRepository userRepository;

        public ChangePasswordCommandHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<Result<ChangePasswordResult>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.FindUserByEmail(request.Email);
            if (user == null)
                return Result.Failure<ChangePasswordResult>(UserError.NotFound);

            if (!BCrypt.Net.BCrypt.Verify(request.OldPassword, user.Password))
                return Result.Failure<ChangePasswordResult>(UserError.InvalidPassword);

            if (request.NewPassword != request.ConfirmPassword)
                return Result.Failure<ChangePasswordResult>(UserError.PasswordMismatch);

            user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            await userRepository.UpdateAsync(user);
            await userRepository.SaveAsync();

            return Result.Success(new ChangePasswordResult(new ChangePasswordResponse(true, "Password changed successfully.")));
        }
    }
}
