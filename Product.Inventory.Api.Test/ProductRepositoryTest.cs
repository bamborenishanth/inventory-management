using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Product.Inventory.Api.Test
{
	[TestClass]
	public class ProductRepositoryTests
	{
		private ProductContext _dbContext;
		private ProductRepository _productRepository;

		[TestInitialize]
		public void Initialize()
		{
			var options = new DbContextOptionsBuilder<ProductContext>()
				.UseInMemoryDatabase("ProductTestDb")
				.Options;

			_dbContext = new ProductContext(options);

			var loggerMock = new Mock<ILogger<ProductRepository>>();
			_productRepository = new ProductRepository(loggerMock.Object, _dbContext);

			// Seed the database with some test data
			_dbContext.Products.AddRange(
				new Product { ProductId = 1, Name = "TestProduct 1", Quantity = 10 },
				new Product { ProductId = 2, Name = "TestProduct 2", Quantity = 20 }
			);
			_dbContext.SaveChanges();
		}

		[TestMethod]
		public async Task GetAllAsyncTest()
		{
			var products = await _productRepository.GetAllAsync();
			Assert.IsNotNull(products);
			Assert.AreEqual(2, products.Count);
		}

		[TestMethod]
		public async Task GetByIdAsyncTest()
		{
			var product = await _productRepository.GetByIdAsync(1);
			Assert.IsNotNull(product);
			Assert.AreEqual(1, product.ProductId);
		}

		[TestMethod]
		public async Task AddAsyncTest()
		{
			var newProduct = new Product { Name = "TestProduct 3", Quantity = 30 };
			await _productRepository.AddAsync(newProduct);

			var products = await _productRepository.GetAllAsync();
			Assert.AreEqual(3, products.Count);
			Assert.AreEqual(newProduct.Name, products.Last().Name);
		}

		[TestMethod]
		public async Task UpdateAsyncTest()
		{
			var product = await _productRepository.GetByIdAsync(1);
			product.Quantity = 50;
			await _productRepository.UpdateAsync(product.ProductId, product);

			var updatedProduct = await _productRepository.GetByIdAsync(1);
			Assert.AreEqual(50, updatedProduct.Quantity);
		}

		[TestMethod]
		public async Task DeleteByIdAsyncTest()
		{
			await _productRepository.DeleteByIdAsync(1);

			var products = await _productRepository.GetAllAsync();
			Assert.AreEqual(1, products.Count);
			Assert.AreEqual(2, products.First().ProductId);
		}

		[TestMethod]
		public async Task DeleteByIdAsyncFailureTest()
		{
			var result = await _productRepository.DeleteByIdAsync(999);
			Assert.IsFalse(result);
		}

		[TestCleanup]
		public void Cleanup()
		{
			_dbContext.Dispose();
		}
	}

}
