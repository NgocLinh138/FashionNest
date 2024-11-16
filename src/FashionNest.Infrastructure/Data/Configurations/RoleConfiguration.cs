namespace FashionNest.Infrastructure.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasConversion(
                roleId => roleId.Value,
                dbId => RoleId.Of(dbId));

            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
}
