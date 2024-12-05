using AutoMapper;
using FashionNest.Application.Common.Messaging;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;
using FashionNest.Domain.Constants;

namespace FashionNest.Application.Features.Roles.Commands.DeleteRole
{
    public class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand, Guid>
    {
        private readonly IRoleRepository roleRepository;
        private readonly IMapper mapper;

        public DeleteRoleCommandHandler(IRoleRepository roleRepository, IMapper mapper)
        {
            this.roleRepository = roleRepository;
            this.mapper = mapper;
        }

        public async Task<Result<Guid>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await roleRepository.GetByIdAsync(RoleId.Of(request.Id));

            if (role == null)
                return Result.Failure<Guid>(RoleError.NotFound);

            try
            {
                await roleRepository.DeleteAsync(role);
                await roleRepository.SaveAsync();

                return Result.Success(role.Id.Value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
