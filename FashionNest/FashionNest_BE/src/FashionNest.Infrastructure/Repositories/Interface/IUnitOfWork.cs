namespace FashionNest.Infrastructure.Repositories.Interface
{
    public interface IUnitOfWork
    {
        RoleRepository RoleRepository { get; }
        void Save();
    }
}
