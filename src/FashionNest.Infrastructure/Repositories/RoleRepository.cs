using FashionNest.Domain.Common.Repositories;

namespace FashionNest.Infrastructure.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Role?> FindRoleByNameAsync(string roleName)
        {
            return await context.Roles.FirstOrDefaultAsync(x => x.Name.ToLower() == roleName.ToLower());
        }
    }
}
