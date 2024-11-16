using AutoMapper;
using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Roles.Responses;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;
using FashionNest.Domain.Constants;

namespace FashionNest.Application.Features.Roles.Commands.UpdateRole
{
    public class UpdateRoleCommandHandler : ICommandHandler<UpdateRoleCommand, UpdateRoleResult>
    {
        private readonly IRoleRepository roleRepository;
        private readonly IMapper mapper;

        public UpdateRoleCommandHandler(IRoleRepository roleRepository, IMapper mapper)
        {
            this.roleRepository = roleRepository;
            this.mapper = mapper;
        }

        public async Task<Result<UpdateRoleResult>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await roleRepository.GetByIdAsync(RoleId.Of(request.Id));

            if (role == null)
                return Result.Failure<UpdateRoleResult>(RoleError.NotFound);

            try
            {
                role.Update(request.Name, request.Description);
                await roleRepository.UpdateAsync(role);
                await roleRepository.SaveAsync();

                var response = new RoleResponse(role.Id.Value, role.Name, role.Description);

                return Result.Success(new UpdateRoleResult(response));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
