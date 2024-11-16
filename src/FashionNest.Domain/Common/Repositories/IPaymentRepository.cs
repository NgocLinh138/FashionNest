namespace FashionNest.Domain.Common.Repositories
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<User?> FindByUserId(Guid userId);
    }
}
