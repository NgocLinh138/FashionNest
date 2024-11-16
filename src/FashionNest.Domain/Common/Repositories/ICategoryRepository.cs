namespace FashionNest.Domain.Common.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category?> GetByNameAsync(string categoryName);
    }
}
