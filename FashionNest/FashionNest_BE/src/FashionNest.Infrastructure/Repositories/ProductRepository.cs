using FashionNest.Domain.Common.Repositories;

namespace FashionNest.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            
        }

        public async Task<IEnumerable<Product>> GetByCategoryIdAsync(CategoryId categoryId)
        {
            return await context.Products
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(CategoryId categoryId)
        {
            return await context.Categories.FindAsync(CategoryId.Of(categoryId.Value));
        }
    }
}
