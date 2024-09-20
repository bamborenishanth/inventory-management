using Microsoft.EntityFrameworkCore;

namespace Product.Inventory.Api
{
	public class ProductRepository
	{
		private readonly ProductContext _context;

		public ProductRepository(ProductContext context)
		{
			_context = context;
		}

		public async Task<List<Product>> GetAll()
		{
			return await _context.Products.ToListAsync().ConfigureAwait(false);
		}

		public async Task<Product> GetById(int productId) => await _context.Products.FindAsync(productId).ConfigureAwait(false);

		public async Task<bool> Add(Product product)
		{
			_context.Products.Add(product);
			int count = await _context.SaveChangesAsync().ConfigureAwait(false);
			return count > 0;
		}

		public async Task<bool> Update(Product product)
		{
			_context.Products.Update(product);
			int count = await _context.SaveChangesAsync().ConfigureAwait(false);
			return count > 0;

		}

		public async Task<bool> Delete(int productId)
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
