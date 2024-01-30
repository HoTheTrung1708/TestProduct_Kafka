namespace Baitaptest.Models
{
    public class ProductDbContext:DbContext
    {
        public ProductDbContext (DbContextOptions<ProductDbContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("table_product");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
