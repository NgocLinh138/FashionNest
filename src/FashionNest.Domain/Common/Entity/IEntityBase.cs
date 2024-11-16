namespace FashionNest.Domain.Common.Entity
{
    public interface IEntityBase<T> : IEntity
    {
        public T Id { get; set; }
    }

    public interface IEntity
    {
        DateTime CreatedAt { get; }
        DateTime? UpdatedAt { get; }
    }
}
