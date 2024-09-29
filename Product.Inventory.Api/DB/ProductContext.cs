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
		}
	}

}
