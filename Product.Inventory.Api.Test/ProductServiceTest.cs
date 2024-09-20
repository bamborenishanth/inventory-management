using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Product.Inventory.Api.Test
{
	[TestClass]
	public class ProductServiceTests
	{
		private Mock<IProductRepository> _mockRepository;
		private ProductService _productService;

		[TestInitialize]
		public void Setup()
		{
			_mockRepository = new Mock<IProductRepository>();
			var mapperMock = new Mock<IMapper>();
			_productService = new ProductService(_mockRepository.Object, mapperMock.Object);
		}

		[TestMethod]
		public async Task GetAllProductsTest()
		{
			// Arrange
			List<Product> products = new List<Product>
			{
				new Product { ProductId = 100001, Name = "TestProduct 1" },
				new Product { ProductId = 100002, Name = "TestProduct 2" }
			};

			_mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

			// Act
			List<Product> result = await _productService.GetAllProducts();

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(2, result.Count);
		}

		[TestMethod]
		public async Task GetProductByIdTest()
		{
			// Arrange
			Product product = new Product { ProductId = 100001, Name = "TestProduct 1" };

			_mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(product);

			// Act
			Product result = await _productService.GetProductById(100001);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(100001, result.ProductId);

			// Arrange
			_mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product)null);

			result = await _productService.GetProductById(100001);

			// Assert
			Assert.IsNull(result);

		}

		[TestMethod]
		public async Task GetProductByIdFailureTest()
		{
			// Arrange
			_mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product)null);

			// Act
			Product result = await _productService.GetProductById(100001);

			// Assert
			Assert.IsNull(result);
		}

		[TestMethod]
		public async Task AddProductTest()
		{
			// Arrange
			Product product = new Product { ProductId = 100003, Name = "TestProduct 3" };

			_mockRepository.Setup(r => r.AddAsync(It.IsAny<Product>())).ReturnsAsync(true);

			// Act
			bool result = await _productService.AddProduct(product);

		}

		[TestMethod]
		public async Task AddProductFailureTest()
		{
			// Arrange
			Product product = new Product { ProductId = 100003, Name = "TestProduct 3" };
			_mockRepository.Setup(r => r.AddAsync(It.IsAny<Product>())).ReturnsAsync(false);

			// Act
			bool result = await _productService.AddProduct(product);
			// Assert
			Assert.IsFalse(result);

		}

		[TestMethod]
		public async Task UpdateProductTest()
		{
			// Arrange
			ProductDto productDto = new ProductDto { ProductId = 100002, Name = "Updated TestProduct 2" };
			Product existingProduct = new Product { ProductId = 100002, Name = "TestProduct 2" };
			Product updatedProduct = new Product { ProductId = 100002, Name = "Updated TestProduct 2" };

			_mockRepository.Setup(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<Product>())).ReturnsAsync(existingProduct);
			_mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(existingProduct);

			// Act
			Product product = await _productService.UpdateProduct(100002, productDto);

		}

		[TestMethod]
		public async Task UpdateProductFailureTest()
		{
			// Arrange
			ProductDto productDto = new ProductDto { ProductId = 100002, Name = "Updated TestProduct 2" };
			_mockRepository.Setup(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<Product>())).ReturnsAsync((Product)null);
			_mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product)null);

			// Act
			Product product = await _productService.UpdateProduct(100002, productDto);

			// Assert
			Assert.IsNull(product);

		}


		[TestMethod]
		public async Task UpdateStockTest()
		{
			// Arrange
			Product productDto = new Product { ProductId = 100001, Name = "TestProduct 1", Quantity = 5 };
			Product updatedProduct = new Product { ProductId = 100001, Name = "TestProduct 1", Quantity = 15 };

			int newQuantity = 10;

			_mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(productDto);
			_mockRepository.Setup(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<Product>())).ReturnsAsync(updatedProduct);

			// Act
			Product product = await _productService.UpdateStock(productDto.ProductId, newQuantity, true);

			// Assert
			Assert.IsNotNull(product);
			Assert.AreEqual(15, product.Quantity);
		}

		[TestMethod]
		public async Task UpdateStockFailureTest()
		{
			// Arrange
			_mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product)null);

			// Act
			Product product = await _productService.UpdateStock(100001, 10, true);


			// Assert
			Assert.IsNull(product);
		}

		[TestMethod]
		public async Task DeleteProductTest()
		{
			// Arrange
			_mockRepository.Setup(r => r.DeleteByIdAsync(It.IsAny<int>())).ReturnsAsync(true);

			// Act
			bool result = await _productService.DeleteProduct(100001);

			// Assert
			Assert.IsTrue(result);

			// Arrange
			_mockRepository.Setup(r => r.DeleteByIdAsync(It.IsAny<int>())).ReturnsAsync(false);

			// Act
			result = await _productService.DeleteProduct(100001);

			// Assert
			Assert.IsFalse(result);

		}

		[TestMethod]
		public async Task DeleteProductFailureTest()
		{
			// Arrange
			_mockRepository.Setup(r => r.DeleteByIdAsync(It.IsAny<int>())).ReturnsAsync(false);

			// Act
			bool result = await _productService.DeleteProduct(100001);

			// Assert
			Assert.IsFalse(result);

		}
	}
}
