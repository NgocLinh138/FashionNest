namespace FashionNest.Domain.Common.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> FindUserByEmail(string email);
        Task<Role> GetRoleByUserId(RoleId roleId);
    }
}
