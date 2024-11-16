using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Roles.Responses;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;
using FashionNest.Domain.Constants;
using Microsoft.Extensions.Logging;

namespace FashionNest.Application.Features.Roles.Queries.GetRoleById
{
    public class GetRoleByIdQueryHandler : IQueryHandler<GetRoleByIdQuery, GetRoleResult>
    {
        private readonly IRoleRepository roleRepository;
        private readonly ILogger<GetRoleByIdQueryHandler> logger;

        public GetRoleByIdQueryHandler(IRoleRepository roleRepository, ILogger<GetRoleByIdQueryHandler> logger)
        {
            this.roleRepository = roleRepository;
            this.logger = logger;
        }

        public async Task<Result<GetRoleResult>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Get role by id {RoleId}", request.Id);

            var role = await roleRepository.GetByIdAsync(RoleId.Of(request.Id));

            if (role == null)
                return Result.Failure<GetRoleResult>(RoleError.NotFound);

            var response = new RoleResponse(role.Id.Value, role.Name, role.Description);

            return Result.Success(new GetRoleResult(response));
        }
    }
}
