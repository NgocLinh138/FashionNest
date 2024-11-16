namespace FashionNest.Infrastructure.Data.Configurations
{
    public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.ToTable("Coupons");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasConversion(
                couponId => couponId.Value,
                dbId => CouponId.Of(dbId));

            builder.Property(x => x.DiscountAmount)
                .HasColumnType("decimal(18,2)") 
                .IsRequired();

            builder.HasMany(x => x.Orders)
               .WithOne(x => x.Coupon)
               .HasForeignKey(x => x.CouponId)
               .IsRequired();

        }
    }
}
