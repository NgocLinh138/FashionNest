using FashionNest.Domain.Common.Repositories;

namespace FashionNest.Infrastructure.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User?> FindByUserId(Guid userId)
        {
            return await context.Users.FirstOrDefaultAsync(x => x.Id.Value == userId);
        }
    }
}
