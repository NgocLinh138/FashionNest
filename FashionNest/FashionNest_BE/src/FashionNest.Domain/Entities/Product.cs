using FashionNest.Domain.Common.Entity;

namespace FashionNest.Domain.Entities
{
    public class Product : EntityBase<ProductId>
    {
        public Product(string name, string? description, decimal price, int stock, string? image, bool isRental, CategoryId categoryId)
        {
            Id = ProductId.Of(Guid.NewGuid());
            Name = name;
            Description = description;
            Price = price;
            Stock = stock;
            Image = image;
            IsRental = isRental;
            CategoryId = categoryId;
        }

        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? Image { get; set; }
        public bool IsRental { get; set; } // True nếu sản phẩm có thể thuê
        public CategoryId CategoryId { get; set; }

        public virtual Category Category { get; set; }


        public void Update(string name, string? description, decimal price, int stock, string? image, bool isRental, CategoryId categoryId)
        {
            Name = name;
            Description = description;
            Price = price;
            Stock = stock;
            Image = image;
            IsRental = isRental;
            CategoryId = categoryId;
        }
    }
}
