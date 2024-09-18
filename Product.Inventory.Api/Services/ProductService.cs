using Microsoft.EntityFrameworkCore;

namespace Product.Inventory.Api
{
	public class ProductService : IProductService
	{
		private readonly ProductContext _context;

		public ProductService(ProductContext context)
		{
			_context = context;
		}

		public async Task<List<Product>> GetAllProducts()
		{
			return await _context.Products.ToListAsync().ConfigureAwait(false);
		}

		public async Task<Product> GetProductById(int productId) => await _context.Products.FindAsync(productId).ConfigureAwait(false);

		public async Task<bool> AddProduct(Product product)
		{
			_context.Products.Add(product);
			int count = await _context.SaveChangesAsync().ConfigureAwait(false);
			return count > 0;
		}

		public async Task<bool> UpdateProduct(Product product)
		{
			_context.Products.Update(product);
			int count = await _context.SaveChangesAsync().ConfigureAwait(false);
			return count > 0;

		}

		public async Task<bool> DeleteProduct(int productId)
		{
			var product = _context.Products.Find(productId);
			if (product != null)
			{
				_context.Products.Remove(product);
				int count = await _context.SaveChangesAsync().ConfigureAwait(false);
				return count > 0;
			}
			return false;


		}

		public async Task<Product> UpdateStock(int productId, int quantity)
		{
			Product product = _context.Products.Find(productId);
			if (product != null)
			{
				product.Quantity = product.Quantity += quantity;
				_context.Products.Update(product);
				int count = await _context.SaveChangesAsync().ConfigureAwait(false);
			}

			return product;
		}
	}

}
