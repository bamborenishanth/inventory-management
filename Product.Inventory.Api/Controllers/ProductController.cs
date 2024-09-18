using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Product.Inventory.Api.Controllers
{
	[Route("api/products")]
	[ApiController]
	[ApiVersion("1.0")]
	public class ProductController : ControllerBase
	{
		private readonly IProductService _productService;

		public ProductController(IProductService productService)
		{
			_productService = productService;
		}

		[HttpGet]
		[MapToApiVersion("1.0")]
		public async Task<IActionResult> GetAllProducts()
		{
			IEnumerable<Product> products = await _productService.GetAllProducts();
			return Ok(products);
		}

		[HttpGet("{productId}")]
		[MapToApiVersion("1.0")]
		public async Task<IActionResult> GetProductById(int productId)
		{
			var product = await _productService.GetProductById(productId);
			if (product == null)
			{
				return NotFound($"Product with {productId} doesn't exist");
			}
			return Ok(product);
		}

		[HttpPost]
		[MapToApiVersion("1.0")]
		public async Task<IActionResult> AddProduct([FromBody] Product product)
		{
			if (product == null)
			{
				return BadRequest("Product cannot be null");
			}

			bool result = await _productService.AddProduct(product);
			if (result)
			{
				return Ok("Product added successfully");
			}
			return StatusCode(500, "A problem occurred while adding the product.");
		}

		[HttpPut("{productId}")]
		[MapToApiVersion("1.0")]
		public async Task<IActionResult> UpdateProduct(int productId, [FromBody] Product product)
		{
			if (product == null || productId != product.ProductId)
			{
				return BadRequest("Product Id mismatch");
			}

			var existingProduct = await _productService.GetProductById(productId);
			if (existingProduct == null)
			{
				return NotFound("Invalid Product Id. There's no product with this id");
			}

			var result = await _productService.UpdateProduct(product);
			if (result)
			{
				return Ok("Product updated successfully");
			}

			return StatusCode(500, "A problem occurred while updating the product.");
		}


		[HttpDelete("{productId}")]
		[MapToApiVersion("1.0")]
		public async Task<IActionResult> DeleteProduct(int productId)
		{
			var result = await _productService.DeleteProduct(productId);
			if (result)
			{
				return Ok("Product deleted successfully");
			}
			return NotFound("Invalid Product Id. There's no product with this id");
		}

		[HttpPut("add-to-stock/{productId}/{quantity}")]
		[MapToApiVersion("1.0")]
		public async Task<IActionResult> AddStock(int productId, int quantity)
		{
			return await UpdateStock(productId, quantity).ConfigureAwait(false);
		}

		[HttpPut("decrement-stock/{productId}/{quantity}")]
		[MapToApiVersion("1.0")]
		public async Task<IActionResult> DecrementStock(int productId, int quantity)
		{
			return await UpdateStock(productId, quantity).ConfigureAwait(false);
		}

		public async Task<IActionResult> UpdateStock(int id, int quantity)
		{
			var product = await _productService.UpdateStock(id, quantity);
			if (product == null)
			{
				return NotFound("Invalid Product Id. There's no product with this id");
			}
			return Ok(product);
		}

	}
}
