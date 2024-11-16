using System.Linq.Expressions;

namespace FashionNest.Domain.Common.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, object>> orderBy = null,
            bool? orderByAsc = true,
            string includeProperties = null,
            int? pageIndex = null,
            int? pageSize = null);

        Task<TEntity> GetByIdAsync(object id);
        Task InsertAsync(TEntity entity);
        Task DeleteAsync(object id);
        Task DeleteAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task SaveAsync();
    }
}
