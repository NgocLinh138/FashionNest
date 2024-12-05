using FashionNest.Domain.Common.Repositories;

namespace FashionNest.Infrastructure.Repositories
{
    public class CouponRepository : GenericRepository<Coupon>, ICouponRepository
    {
        public CouponRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
