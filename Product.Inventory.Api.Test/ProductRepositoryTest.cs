using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Product.Inventory.Api.Test
{
	[TestClass]
	public class ProductRepositoryTests
	{
		private Mock<ILogger<ProductRepository>> _mockLogger;
		private ProductContext _context;
		private ProductRepository _productRepository;

		[TestInitialize]
		public void Setup()
		{
			var options = new DbContextOptionsBuilder<ProductContext>()
							.UseInMemoryDatabase(databaseName: "TestProductDb")
							.Options;

			_context = new ProductContext(options);
			_mockLogger = new Mock<ILogger<ProductRepository>>();
			_productRepository = new ProductRepository(_mockLogger.Object, _context);
		}

		[TestCleanup]
		public void Cleanup()
		{
			_context.Database.EnsureDeleted();
			_context.Dispose();
		}

		[TestMethod]
		public async Task GetAllAsyncReturnsAllProducts()
		{
			// Arrange
			_context.Products.AddRange(new Product { ProductId = 1, Name = "Product1" },
											   new Product { ProductId = 2, Name = "Product2" });
			await _context.SaveChangesAsync();

			// Act
			var result = await _productRepository.GetAllAsync();

			// Assert
			Assert.AreEqual(2, result.Count);
			Assert.AreEqual("Product1", result[0].Name);
		}

		[TestMethod]
		public async Task GetAllAsyncReturnsEmptyListWhenNoProductsExist()
		{
			// Act
			var result = await _productRepository.GetAllAsync();

			// Assert
			Assert.AreEqual(0, result.Count);
		}

		[TestMethod]
		public async Task GetByIdAsyncReturnsProductWhenIdIsFound()
		{
			// Arrange
			Product product = new Product { ProductId = 1, Name = "Product1" };
			_context.Products.Add(product);
			await _context.SaveChangesAsync();

			// Act
			Product result = await _productRepository.GetByIdAsync(1);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual("Product1", result.Name);
		}

		[TestMethod]
		public async Task GetByIdAsyncReturnsNullWhenProductIdIsNotFound()
		{
			// Act
			Product result = await _productRepository.GetByIdAsync(10);

			// Assert
			Assert.IsNull(result);
		}

		[TestMethod]
		public async Task AddAsyncSucessTest()
		{
			// Arrange
			Product product = new Product { ProductId = 3, Name = "Product3" };

			// Act
			bool result = await _productRepository.AddAsync(product);

			// Assert
			Assert.IsTrue(result);
			Product? addedProduct = await _context.Products.FindAsync(3);
			Assert.IsNotNull(addedProduct);
			Assert.AreEqual("Product3", addedProduct.Name);
		}

		[TestMethod]
		public async Task AddAsyncFailureTest()
		{
			// Act
			Product invalidProduct = new Product();
			bool result = await _productRepository.AddAsync(invalidProduct);

			// Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public async Task UpdateAsyncSuccessTest()
		{
			// Arrange
			Product product = new Product { ProductId = 1, Name = "OldProduct" };
			_context.Products.Add(product);
			await _context.SaveChangesAsync();

			// Act
			Product? existingProduct = await _context.Products.FindAsync(1);
			existingProduct.Name = "UpdatedProduct";
			Product result = await _productRepository.UpdateAsync(1, existingProduct);

			// Assert
			Assert.AreEqual("UpdatedProduct", result.Name);
		}

		[TestMethod]
		public async Task UpdateAsyncFailureTest()
		{
			// Act
			Product product = new Product { ProductId = 1, Name = "UpdatedProduct" };
			Product updatedProduct = await _productRepository.UpdateAsync(1, product);

			// Assert
			Assert.IsNull(updatedProduct);
		}


		[TestMethod]
		public async Task DeleteAsyncSuccessTest()
		{
			// Arrange
			Product product = new Product { ProductId = 1, Name = "Product1" };
			_context.Products.Add(product);
			await _context.SaveChangesAsync();

			// Act
			var result = await _productRepository.DeleteByIdAsync(1);

			// Assert
			Assert.IsTrue(result);
			Product? deletedProduct = await _context.Products.FindAsync(1);
			Assert.IsNull(deletedProduct);
		}

		[TestMethod]
		public async Task DeleteAsyncFailureTest()
		{
			// Act
			var result = await _productRepository.DeleteByIdAsync(10);

			// Assert
			Assert.IsFalse(result);
		}
	}

}
