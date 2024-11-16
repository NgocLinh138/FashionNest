namespace FashionNest.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasConversion(
                orderId => orderId.Value,
                dbId => OrderId.Of(dbId));

            builder.Property(x => x.Status)
                .HasDefaultValue(OrderStatus.Draft)
                .HasConversion(
                   x => x.ToString(),
                   dbStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), dbStatus));

            builder.Property(x => x.TotalPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.UserId)
                .IsRequired();

            builder.HasOne(x => x.Payment)
                .WithOne()
                .HasForeignKey<Order>(x => x.PaymentId)
                .IsRequired(false);

            builder.HasMany(x => x.OrderItems)
                .WithOne()
                .HasForeignKey(x => x.OrderId)
                .IsRequired();

            builder.HasOne(x => x.Coupon)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.CouponId)
                .IsRequired(false);
        }
    }
}
