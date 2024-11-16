namespace FashionNest.Domain.Common.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<List<Order>> GetOrdersByUserIdAsync(Guid userId);
    }
}
