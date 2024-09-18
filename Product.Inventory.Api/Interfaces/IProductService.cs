using Microsoft.EntityFrameworkCore;

namespace Product.Inventory.Api
{
	public interface IProductService
	{
		public Task<List<Product>> GetAllProducts();

		public Task<Product> GetProductById(int productId);

		public Task<bool> AddProduct(Product product);

		public Task<bool> UpdateProduct(Product product);

		public Task<bool> DeleteProduct(int productId);

		public Task<Product> UpdateStock(int productId, int quantity);

	}
}
