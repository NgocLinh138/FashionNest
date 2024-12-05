using FashionNest.Domain.Common.Repositories;

namespace FashionNest.Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Category?> GetByNameAsync(string categoryName)
        {
            return await context.Categories.FirstOrDefaultAsync(c => c.Name.ToLower() == categoryName.ToLower());
        }
    }
}
