using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Product.Inventory.Api.Test
{
	[TestClass]
	public class ProductContextTest
	{
		private ProductContext GetInMemoryDbContext()
		{
			var options = new DbContextOptionsBuilder<ProductContext>()
				.UseInMemoryDatabase(databaseName: "ProductTestDb")
				.Options;

			return new ProductContext(options);
		}


		[TestMethod]
		public async Task ProductContextCanAddProduct()
		{
			// Arrange
			using (var context = GetInMemoryDbContext())
			{
				var product = new Product
				{
					Name = "TestProduct 1",
					Description = "Test Product 1",
					Quantity = 10,
					Price = 100m
				};

				await context.Products.AddAsync(product).ConfigureAwait(false);
				await context.SaveChangesAsync().ConfigureAwait(false);

				Assert.AreEqual(1, context.Products.Count());
				var retrievedProduct = context.Products.First();
				Assert.AreEqual("TestProduct 1", retrievedProduct.Name);
			}
		}
	}
}
