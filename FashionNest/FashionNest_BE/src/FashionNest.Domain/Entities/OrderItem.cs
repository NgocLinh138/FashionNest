using FashionNest.Domain.Common.Entity;

namespace FashionNest.Domain.Entities
{
    public class OrderItem : EntityBase<OrderItemId>
    {
        internal OrderItem(OrderId orderId, ProductId productId, int quantity, decimal price)
        {
            Id = OrderItemId.Of(Guid.NewGuid());
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
            Price = price;
        }

        public OrderId OrderId { get; set; }    
        public ProductId ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }


        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }


        public void Update(int quantity, decimal price)
        {
            Quantity = quantity;
            Price = price;
        }
    }
}
