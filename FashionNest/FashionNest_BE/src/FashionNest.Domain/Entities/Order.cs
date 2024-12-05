using FashionNest.Domain.Common.Entity;

namespace FashionNest.Domain.Entities
{
    public class Order : EntityBase<OrderId>
    {
        public Order()
        {
            IsDeleted = false; 
        }

        public Order(UserId userId, OrderStatus status, DateTime orderDate, CouponId? couponId)
        {
            Id = OrderId.Of(Guid.NewGuid());
            UserId = userId;
            Status = status;
            OrderDate = orderDate;
            CouponId = couponId;
            OrderItems = new List<OrderItem>();
            IsDeleted = false;
        }


        public UserId UserId { get; set; }
        public PaymentId? PaymentId { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Draft;
        public DateTime OrderDate { get; set; }
        public CouponId? CouponId { get; set; }
        public int CouponUsed { get; set; }
        private decimal _totalPrice;

        public decimal TotalPrice
        {
            get => _totalPrice;
            private set => _totalPrice = value;
        }

        public bool IsDeleted { get; private set; }
        public DateTime? DeleteDate { get; private set; }


        public virtual User User { get; set; }
        public virtual Payment Payment { get; set; }
        public virtual Coupon Coupon { get; set; } 
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();



        private void UpdateTotalPrice()
        {
            TotalPrice = OrderItems.Sum(x => x.Price * x.Quantity);
        }

        public void AddOrderItem(ProductId productId, int quantity, decimal price)
        {
            var orderItem = new OrderItem(Id, productId, quantity, price);
            OrderItems.Add(orderItem);
            UpdateTotalPrice();
        }

        public void UpdateOrderItem(OrderItemId orderItemId, int quantity, decimal price)
        {
            var orderItem = OrderItems.FirstOrDefault(x => x.Id == orderItemId);
            if (orderItem != null)
            {
                orderItem.Update(quantity, price);
                UpdateTotalPrice();
            }
        }

        public void RemoveOrderItem(OrderItemId orderItemId)
        {
            var item = OrderItems.FirstOrDefault(x => x.Id == orderItemId);
            if (item != null)
            {
                OrderItems.Remove(item);
                UpdateTotalPrice(); 
            }
        }

        public void CancelOrder()
        {
            if (Status == OrderStatus.Draft)
            {
                Status = OrderStatus.Cancelled;
            }
        }

        public void CompleteOrder(PaymentId paymentId)
        {
            PaymentId = paymentId;
            Status = OrderStatus.Completed;
        }

        public void UpdateStatus(OrderStatus newStatus)
        {
            if (Status != OrderStatus.Completed)
            {
                Status = newStatus;
            }
        }

        public void SoftDelete()
        {
            if (Status == OrderStatus.Completed || Status == OrderStatus.Cancelled)
                throw new InvalidOperationException("Cannot delete completed or cancelled orders.");

            IsDeleted = true;
            DeleteDate = DateTime.UtcNow; 
        }

        public void ApplyCoupon(decimal discountAmount)
        {
            TotalPrice -= discountAmount;
            CouponUsed++;
        }
    }
}
