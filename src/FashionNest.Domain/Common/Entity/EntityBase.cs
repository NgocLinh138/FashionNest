namespace FashionNest.Domain.Common.Entity
{
    public abstract class EntityBase<T> : IEntityBase<T>
    {
        public T Id { get; set; }
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; private set; }

        public void UpdateTimestamp()
        {
            UpdatedAt = DateTime.Now;
        }
    }
}
