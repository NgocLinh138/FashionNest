using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Services.Interface;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;
using FashionNest.Domain.Constants;

namespace FashionNest.Application.Features.Auth.Queries.Login
{
    public class LoginUserQueryHandler : IQueryHandler<LoginUserQuery, LoginResult>
    {
        private readonly IUserRepository userRepository;
        private readonly IJWTTokenService jWTTokenService;

        public LoginUserQueryHandler(IUserRepository userRepository, IJWTTokenService jWTTokenService)
        {
            this.userRepository = userRepository;
            this.jWTTokenService = jWTTokenService;
        }


        public async Task<Result<LoginResult>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var user = await userRepository.FindUserByEmail(request.Email);
            if (user == null)
                return Result.Failure<LoginResult>(UserError.NotFound);

            bool checkPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);


            if (user.RoleId == null || user.Role == null)
            {
                return Result.Failure<LoginResult>(RoleError.NotFound);
            }

            if (checkPassword)
            {
                return new LoginResult(new LoginResponse(true, "Login Successfully.", user.Role.Name, jWTTokenService.GenerateAccessToken(user)));
            }
            else
            {
                return new LoginResult(new LoginResponse(false, "Login fail."));
            }
        }
    }
}
