namespace FashionNest.Infrastructure.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasConversion(
                categoryId => categoryId.Value,
                dbId => CategoryId.Of(dbId));

            builder.Property(x => x.Name).IsRequired();
        }
    }
}
