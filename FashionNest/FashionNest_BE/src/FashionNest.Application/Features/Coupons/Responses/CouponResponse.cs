namespace FashionNest.Application.Features.Coupons.Responses
{
    public class CouponResponse
    {
        public CouponResponse(Guid id, string code, decimal discountAmount, DateTime expirationDate, bool isActive, string? description)
        {
            Id = id;
            Code = code;
            DiscountAmount = discountAmount;
            ExpirationDate = expirationDate.ToString("dd/MM/yyyy");  
            IsActive = isActive;
            Description = description;
        }

        public Guid Id { get; }
        public string Code { get; }
        public decimal DiscountAmount { get; }
        public string ExpirationDate { get; } 
        public bool IsActive { get; }
        public string? Description { get; }
    }
}
