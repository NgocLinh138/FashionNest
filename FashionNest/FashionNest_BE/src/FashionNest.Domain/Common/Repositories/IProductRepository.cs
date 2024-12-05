namespace FashionNest.Domain.Common.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Category> GetCategoryByIdAsync(CategoryId categoryId);
        Task<IEnumerable<Product>> GetByCategoryIdAsync(CategoryId categoryId);
    }
}
