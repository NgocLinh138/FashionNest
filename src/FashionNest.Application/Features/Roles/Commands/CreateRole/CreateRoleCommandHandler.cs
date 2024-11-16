using AutoMapper;
using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Roles.Responses;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;
using Microsoft.Extensions.Logging;

namespace FashionNest.Application.Features.Roles.Commands.CreateRole
{
    public class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand, CreateRoleResult>
    {
        private readonly IRoleRepository roleRepository;
        private readonly IMapper mapper;
        private readonly ILogger<CreateRoleCommandHandler> logger;

        public CreateRoleCommandHandler(IRoleRepository roleRepository, IMapper mapper, ILogger<CreateRoleCommandHandler> logger)
        {
            this.roleRepository = roleRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Result<CreateRoleResult>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Create a new role {Role}", request);

                var role = Role.Create(request.Name, request.Description);
                await roleRepository.InsertAsync(role);
                await roleRepository.SaveAsync();

                var response = new RoleResponse(role.Id.Value, role.Name, role.Description);

                return Result.Success(new CreateRoleResult(response));           
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
