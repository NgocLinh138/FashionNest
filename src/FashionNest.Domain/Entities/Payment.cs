using FashionNest.Domain.Common.Entity;

namespace FashionNest.Domain.Entities
{
    public class Payment : EntityBase<PaymentId>
    {
        public Payment()
        {
        }

        public Payment(UserId userId, PaymentMethod method, decimal amount, string? description, DateTime paymentDate, PaymentStatus paymentStatus)
        {
            Id = PaymentId.Of(Guid.NewGuid());
            UserId = userId;
            Method = method;
            Amount = amount;
            Description = description;
            PaymentDate = paymentDate;
            Status = paymentStatus;
        }

        public UserId UserId { get; set; }
        public PaymentMethod? Method { get; set; } = PaymentMethod.COD;
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public PaymentStatus? Status { get; set; } = PaymentStatus.Pending;

        public virtual User User { get; set; }
    }
}
