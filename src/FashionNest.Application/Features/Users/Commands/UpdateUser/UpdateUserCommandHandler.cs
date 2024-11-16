using FashionNest.Application.Common.Exceptions;
using FashionNest.Application.Common.Messaging;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;
using FashionNest.Domain.Constants;

namespace FashionNest.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, UpdateUserResult>
    {
        private readonly IUserRepository userRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<Result<UpdateUserResult>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(UserId.Of(request.UserId));
            if (user == null)
                return Result.Failure<UpdateUserResult>(UserError.NotFound);

            if (!user.IsActive)
                throw new BadRequestException("User is inactive.");

            var role = await userRepository.GetRoleByUserId(RoleId.Of(user.RoleId.Value));

            try
            {
                user.Update(request.Name, request.Password, request.PhoneNumber, request.Address);

                await userRepository.UpdateAsync(user);
                await userRepository.SaveAsync();

                var response = new UpdateUserResult(
                    new Responses.UserResponse(
                        user.Id.Value,
                        user.Name,
                        user.Email,
                        user.PhoneNumber,
                        user.Address,
                        user.IsActive,
                        new Responses.UserRoleResponse(
                            role.Id.Value,
                            role.Name)));

                return Result.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
