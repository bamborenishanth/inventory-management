namespace Product.Inventory.Api
{
	public interface IProductService
	{
		public Task<List<Product>> GetAllProducts();

		public Task<Product> GetProductById(int productId);

		public Task<bool> AddProduct(ProductDto product);

		public Task<Product> UpdateProduct(int productId, ProductDto productDto);

		public Task<bool> DeleteProduct(int productId);

		public Task<Product> UpdateStock(int productId, int quantity, bool addStock);


	}
}
