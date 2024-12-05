using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Users.Commands.UpdateUserStatus;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;
using FashionNest.Domain.Constants;

namespace FashionNest.Application.Features.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, Guid>
    {
        private readonly IUserRepository userRepository;

        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<Result<Guid>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(UserId.Of(request.UserId));
            if (user == null)
                return Result.Failure<Guid>(UserError.NotFound);

            try
            {
                user.IsActive = false;

                await userRepository.UpdateAsync(user);
                await userRepository.SaveAsync();

                return Result.Success(user.Id.Value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
