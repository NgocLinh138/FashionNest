using FashionNest.Domain.Common.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FashionNest.Infrastructure.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(Guid userId)
        {
            return await _context.Orders.ToListAsync();
        }
    }
}
