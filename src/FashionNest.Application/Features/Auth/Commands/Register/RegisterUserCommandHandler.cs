using FashionNest.Application.Common.Messaging;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;
using FashionNest.Domain.Constants;

namespace FashionNest.Application.Features.Auth.Commands.Register
{
    public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, RegisterUserResult>
    {
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;

        public RegisterUserCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
        }

        public async Task<Result<RegisterUserResult>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.FindUserByEmail(request.Email);
            if (user != null)
                return Result.Failure<RegisterUserResult>(UserError.UserExisted);

            var customerRole = await roleRepository.FindRoleByNameAsync("Customer");
            if (customerRole == null)
                return Result.Failure<RegisterUserResult>(new Error("User.RoleNotFound", "Customer role not found."));

            await userRepository.InsertAsync(new User()
            {
                Id = UserId.Of(Guid.NewGuid()),
                Name = request.Name,
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                IsActive = true,
                RoleId = RoleId.Of(customerRole.Id.Value)
            });

            await userRepository.SaveAsync();

            return new RegisterUserResult(new RegisterUserResponse(true, "Register successfully"));
        }
    }
}
