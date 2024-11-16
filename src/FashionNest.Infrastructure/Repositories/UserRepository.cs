using FashionNest.Domain.Common.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FashionNest.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User> FindUserByEmail(string email)
            => await context.Users.FirstOrDefaultAsync(x => x.Email == email);

        public async Task<Role> GetRoleByUserId(RoleId roleId)
            => await context.Roles.FindAsync(RoleId.Of(roleId.Value));
    }
}
