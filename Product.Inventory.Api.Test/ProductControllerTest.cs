using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Product.Inventory.Api.Test
{
	[TestClass]
	public class ProductControllerTest
	{
		private Mock<IProductService> _mockProductService;
		private ProductController _controller;

		[TestInitialize]
		public void TestInitialize()
		{
			_mockProductService = new Mock<IProductService>();
			_controller = new ProductController(_mockProductService.Object);
		}

		[TestMethod]
		public async Task GetAllProductsTest()
		{
			// Arrange
			var products = new List<Product>
			{
				new Product { ProductId = 1, Name = "Product1", Price = 100, Quantity = 10 },
				new Product { ProductId = 2, Name = "Product2", Price = 200, Quantity = 20 }
			};
			_mockProductService.Setup(service => service.GetAllProducts()).ReturnsAsync(products);

			// Act
			IActionResult result = await _controller.GetAllProducts();

			// Assert
			OkObjectResult? okResult = result as OkObjectResult;
			Assert.IsNotNull(okResult);
			List<Product> returnValue = okResult.Value as List<Product>;
			Assert.AreEqual(2, returnValue.Count);
		}

		[TestMethod]
		public async Task GetProductById()
		{
			// Arrange
			var product = new Product { ProductId = 1, Name = "Product1", Price = 100, Quantity = 10 };
			_mockProductService.Setup(service => service.GetProductById(1)).ReturnsAsync(product);

			// Act
			IActionResult result = await _controller.GetProductById(1);

			// Assert
			OkObjectResult? okResult = result as OkObjectResult;
			Assert.IsNotNull(okResult);
			Product? returnValue = okResult.Value as Product;
			Assert.AreEqual(1, returnValue.ProductId);

		}

		[TestMethod]
		public async Task GetProductByInvalidIdTest()
		{
			// Arrange
			_mockProductService.Setup(service => service.GetProductById(1)).ReturnsAsync((Product)null);

			// Act
			var result = await _controller.GetProductById(1);

			// Assert
			Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
		}

		[TestMethod]
		public async Task AddProductTest()
		{
			// Arrange
			var product = new Product { Name = "New Product", Price = 10, Quantity = 5 };
			_mockProductService.Setup(service => service.AddProduct(product)).ReturnsAsync(true);

			// Act
			var result = await _controller.AddProduct(product);

			// Assert
			Assert.IsInstanceOfType(result, typeof(OkObjectResult));
		}

		[TestMethod]
		public async Task AddInvalidProductTest()
		{
			// Act
			var result = await _controller.AddProduct(null);

			// Assert
			Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
		}

		[TestMethod]
		public async Task UpdateProductWithWrongIdTest()
		{
			// Arrange
			var product = new ProductDto { ProductId = 1, Name = "Product1" };

			// Act
			var result = await _controller.UpdateProduct(2, product);

			// Assert
			Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
		}

		[TestMethod]
		public async Task DeleteValidProductTest()
		{
			// Arrange
			_mockProductService.Setup(service => service.DeleteProduct(1)).ReturnsAsync(true);

			// Act
			var result = await _controller.DeleteProduct(1);

			// Assert
			Assert.IsInstanceOfType(result, typeof(OkObjectResult));
		}

		[TestMethod]
		public async Task DeleteInvalidProductTest()
		{
			// Arrange
			_mockProductService.Setup(service => service.DeleteProduct(1)).ReturnsAsync(false);

			// Act
			var result = await _controller.DeleteProduct(1);

			// Assert
			Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
		}

		//[TestMethod]
		//public async Task UpdateStockTest()
		//{
		//	// Arrange
		//	var product = new Product { ProductId = 1, Quantity = 10 };
		//	_mockProductService.Setup(service => service.GetProductById(It.IsAny<int>())).ReturnsAsync(product);
		//	_mockProductService.Setup(service => service.UpdateStock(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(product);


		//	// Act
		//	var result = await _controller.AddStock(1, 5);

		//	// Assert
		//	var okResult = result as OkObjectResult;
		//	Assert.IsNotNull(okResult);
		//	var returnValue = okResult.Value as Product;
		//	Assert.AreEqual(1, returnValue.ProductId);
		//	Assert.AreEqual(10, returnValue.Quantity);
		//}
	}
}
