using FashionNest.Domain.Common.Entity;

namespace FashionNest.Domain.Entities
{
    public class Coupon : EntityBase<CouponId>
    {
        public Coupon(string code, decimal discountAmount, DateTime expirationDate, bool isActive, string? description,
                 decimal? minOrderAmount, int? maxUses, int? maxUsesPerUser, bool isNewCustomerOnly)
        {
            Id = CouponId.Of(Guid.NewGuid());
            Code = code;
            DiscountAmount = discountAmount;
            ExpirationDate = expirationDate;
            IsActive = isActive;
            Description = description;
            MinOrderAmount = minOrderAmount;
            MaxUses = maxUses;
            MaxUsesPerUser = maxUsesPerUser;
            IsNewCustomerOnly = isNewCustomerOnly;
            UsedCount = 0;
        }

        public string Code { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public string? Description { get; set; }
        public decimal? MinOrderAmount { get; set; }
        public int? MaxUses { get; set; }
        public int? MaxUsesPerUser { get; set; }
        public bool IsNewCustomerOnly { get; set; }
        public int UsedCount { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();


        // Validation checking only the coupon status and expiration
        public bool IsValid()
        {
            return IsActive && ExpirationDate > DateTime.Now && UsedCount < MaxUses;
        }

        // Validation that includes order-specific logic and user order history
        public bool IsValid(Order order, List<Order> userOrders)
        {
            // Check if coupon is active and not expired
            if (!IsActive || ExpirationDate < DateTime.Now || UsedCount >= MaxUses)
                return false;

            // Check if coupon exceeds the maximum usage per user
            int userCouponUsageCount = userOrders
                .Where(o => o.UserId == order.UserId) 
                .Count(o => o.CouponId == this.Id); // Coupon usage count per user

            if (MaxUsesPerUser.HasValue && userCouponUsageCount >= MaxUsesPerUser.Value)
                return false;

            // Check if the order meets the minimum amount requirement
            if (MinOrderAmount.HasValue && order.TotalPrice < MinOrderAmount.Value)
                return false;

            return true;
        }

        public bool CanApplyToOrder(decimal orderAmount, int userUses)
        {
            if (!IsValid()) return false;
            if (MinOrderAmount.HasValue && orderAmount < MinOrderAmount) return false;
            //if (MaxUses.HasValue && UsedCount >= MaxUses) return false;
            if (MaxUsesPerUser.HasValue && userUses >= MaxUsesPerUser) return false;
            return true;
        }

        public void ApplyToOrder(Order order)
        {
            order.ApplyCoupon(DiscountAmount);
            if (MaxUses.HasValue)
                UsedCount++;

            //if (MaxUsesPerUser.HasValue)
            //    MaxUsesPerUser--;
        }

        public void Update(string code, decimal discountAmount, DateTime expirationDate, bool isActive, string? description)
        {
            Code = code;
            DiscountAmount = discountAmount;
            ExpirationDate = expirationDate;
            IsActive = isActive;
            Description = description;
        }
    }
}
