using Microsoft.AspNetCore.Mvc;

namespace Product.Inventory.Api
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
			List<Product> products = await _productService.GetAllProducts();
			return Ok(products);
		}

		[HttpGet("{productId}")]
		[MapToApiVersion("1.0")]
		public async Task<IActionResult> GetProductById(int productId)
		{
			Product product = await _productService.GetProductById(productId);
			if (product == null)
			{
				return NotFound($"Product with id-{productId} doesn't exist");
			}
			return Ok(product);
		}

		[HttpPost]
		[MapToApiVersion("1.0")]
		public async Task<IActionResult> AddProduct([FromBody] ProductDto productDto)
		{
			if (productDto == null)
			{
				return BadRequest("Product cannot be null");
			}

			if (!ModelState.IsValid)
			{
				List<string> errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

				return BadRequest(errors);
			}

			bool result = await _productService.AddProduct(productDto);
			if (result)
			{
				return Ok("Product added successfully");
			}
			return StatusCode(500, "A problem occurred while adding the product.");
		}

		[HttpPut("{productId}")]
		[MapToApiVersion("1.0")]
		public async Task<IActionResult> UpdateProduct(int productId, [FromBody] ProductDto productDto)
		{
			if (productDto == null)
			{
				return BadRequest("Product cannot be null");
			}

			if (!ModelState.IsValid)
			{
				List<string> errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

				return BadRequest(errors);
			}

			Product existingProduct = await _productService.GetProductById(productId);
			if (existingProduct == null)
			{
				return NotFound($"Invalid Product Id. There's no product with this id-{productId}");
			}

			Product updatedProduct = await _productService.UpdateProduct(productId, productDto);
			if (updatedProduct != null)
			{
				return Ok(updatedProduct);
			}

			return StatusCode(500, "A problem occurred while updating the product.");
		}

		[HttpDelete("{productId}")]
		[MapToApiVersion("1.0")]
		public async Task<IActionResult> DeleteProduct(int productId)
		{
			bool result = await _productService.DeleteProduct(productId);
			if (result)
			{
				return Ok("Product deleted successfully");
			}
			return NotFound($"Invalid Product Id. There's no product with this id-{productId}");
		}

		[HttpPut("add-to-stock/{productId}/{quantity}")]
		[MapToApiVersion("1.0")]
		public async Task<IActionResult> AddStock(int productId, int quantity)
		{
			return await UpdateStock(productId, quantity, true);
		}

		[HttpPut("decrement-stock/{productId}/{quantity}")]
		[MapToApiVersion("1.0")]
		public async Task<IActionResult> DecrementStock(int productId, int quantity)
		{
			return await UpdateStock(productId, quantity, false);
		}

		private async Task<IActionResult> UpdateStock(int productId, int quantity, bool addStock)
		{
			Product product = await _productService.GetProductById(productId);
			if (product == null)
			{
				return NotFound($"Invalid Product Id. There's no product with this id-{productId}");
			}

			Product updatedProduct = await _productService.UpdateStock(productId, quantity, addStock);
			if (updatedProduct != null)
			{
				return Ok(updatedProduct);
			}

			return StatusCode(500, "A problem occurred while updating the quantity of the product.");
		}
	}
}
