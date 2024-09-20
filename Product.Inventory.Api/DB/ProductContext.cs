using Microsoft.EntityFrameworkCore;

namespace Product.Inventory.Api
{
	public class ProductContext : DbContext
	{
		public ProductContext(DbContextOptions<ProductContext> options)
			: base(options)
		{
		}

		public DbSet<Product> Products { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Product>()
			.HasKey(p => p.ProductId);

			modelBuilder.HasSequence<int>("product_id_seq")
				.StartsAt(100000)
				.IncrementsBy(1)
				.HasMin(100000)
				.HasMax(999999)
				.IsCyclic();

			modelBuilder.Entity<Product>()
				.Property(p => p.ProductId)
				.HasDefaultValueSql("nextval('product_id_seq')");

			modelBuilder.Entity<Product>()
				.Property(p => p.Name)
				.IsRequired()
				.HasMaxLength(20);

			modelBuilder.Entity<Product>()
				.Property(p => p.Description)
				.HasMaxLength(100);

			modelBuilder.Entity<Product>()
				.Property(p => p.Quantity)
				.HasDefaultValue(1);

			modelBuilder.Entity<Product>()
				.HasCheckConstraint("CK_Product_Quantity_NonNegative", "Quantity >= 0");

			modelBuilder.Entity<Product>()
				.Property(p => p.Price)
				.HasColumnType("decimal(10,2)");
		}
	}

}
