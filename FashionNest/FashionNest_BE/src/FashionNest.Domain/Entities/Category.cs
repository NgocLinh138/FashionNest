using FashionNest.Domain.Common.Entity;
using FashionNest.Domain.ValueObjects;

namespace FashionNest.Domain.Entities
{
    public class Category : EntityBase<CategoryId>
    {
        public Category(string name, string description)
        {
            Id = CategoryId.Of(Guid.NewGuid());
            Name = name;
            Description = description;
        }

        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();


        public void Update(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
