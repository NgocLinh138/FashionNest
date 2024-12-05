namespace FashionNest.Domain.Common.Repositories
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<Role?> FindRoleByNameAsync(string roleName);
    }
}
