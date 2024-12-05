namespace FashionNest.Infrastructure.Data.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasConversion(
                paymentId => paymentId.Value,
                dbId => PaymentId.Of(dbId));

            builder.Property(x => x.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(x => x.Status)
                .HasDefaultValue(PaymentStatus.Pending)
                .HasConversion(
                   x => x.ToString(),
                   dbStatus => (PaymentStatus)Enum.Parse(typeof(PaymentStatus), dbStatus));

            builder.Property(x => x.Method)
                .HasDefaultValue(PaymentMethod.COD)
                .HasConversion(
                    x => x.ToString(),
                    dbMethod => (PaymentMethod)Enum.Parse(typeof(PaymentMethod), dbMethod));

            builder.Property(x => x.CreatedAt).IsRequired().HasDefaultValue(DateTime.Now);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Payments)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
        }
    }
}
