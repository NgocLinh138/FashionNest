using AutoMapper;
using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Roles.Responses;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;
using Microsoft.Extensions.Logging;

namespace FashionNest.Application.Features.Roles.Queries.GetRole
{
    public class GetRoleQueryHandler : IQueryHandler<GetRolesQuery, GetRolesResult>
    {
        private readonly IRoleRepository roleRepository;
        private readonly IMapper mapper;
        private readonly ILogger<GetRoleQueryHandler> logger;

        public GetRoleQueryHandler(IRoleRepository roleRepository, IMapper mapper, ILogger<GetRoleQueryHandler> logger)
        {
            this.roleRepository = roleRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Result<GetRolesResult>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Get all roles {Role}", request);

                var roles = await roleRepository.GetAsync(
                    orderByAsc: true,
                    pageIndex: request.pageIndex,
                    pageSize: request.pageSize);

                var response = mapper.Map<List<RoleResponse>>(roles);

                return Result.Success(new GetRolesResult(new PaginatedResult<RoleResponse>(response, response.Count(), request.pageIndex, request.pageSize)));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
