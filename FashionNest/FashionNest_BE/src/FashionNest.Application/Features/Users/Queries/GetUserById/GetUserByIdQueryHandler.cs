using FashionNest.Application.Common.Messaging;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;
using FashionNest.Domain.Constants;

namespace FashionNest.Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, GetUserByIdResult>
    {
        private readonly IUserRepository userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<Result<GetUserByIdResult>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(UserId.Of(request.Id));
            if (user == null)
                return Result.Failure<GetUserByIdResult>(UserError.NotFound);

            var role = await userRepository.GetRoleByUserId(RoleId.Of(user.RoleId.Value));

            var response = new GetUserByIdResult(
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
    }
}
