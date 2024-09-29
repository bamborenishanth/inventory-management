using Microsoft.EntityFrameworkCore;

namespace Product.Inventory.Api
{
	public class ProductRepository : IProductRepository, IDisposable
	{
		private readonly ILogger<ProductRepository> _logger;
		private readonly ProductContext _context;
		private bool _disposed;

		public ProductRepository(ILogger<ProductRepository> logger, ProductContext dbContext)
		{
			_logger = logger;
			_context = dbContext;
		}

		public async Task<List<Product>> GetAllAsync()
		{
			try
			{
				return await _context.Products.ToListAsync().ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred while fetching all products.");
				return new List<Product>();
			}
		}

		public async Task<Product> GetByIdAsync(int productId)
		{
			try
			{
				return await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId).ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error occurred while fetching the product with id-{productId}.");
				return null;
			}
		}

		public async Task<bool> AddAsync(Product product)
		{
			try
			{
				await _context.Products.AddAsync(product).ConfigureAwait(false);
				await _context.SaveChangesAsync().ConfigureAwait(false);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred while adding the product.");
				return false;
			}
		}

		public async Task<Product> UpdateAsync(int productId, Product product)
		{
			try
			{
				_context.Products.Update(product);
				await _context.SaveChangesAsync().ConfigureAwait(false);
				var updatedProduct = await GetByIdAsync(productId).ConfigureAwait(false);

				return updatedProduct;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error occurred while updating the product with id-{productId}.");
				return null;
			}
		}

		public async Task<bool> DeleteByIdAsync(int productId)
		{
			try
			{
				Product product = await GetByIdAsync(productId);
				if (product != null)
				{
					_context.Products.Remove(product);
					await _context.SaveChangesAsync().ConfigureAwait(false);
					return true;
				}

				return false;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error occurred while deleting the product with id-{productId}.");
				return false;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					_context?.Dispose();
				}
				_disposed = true;
			}
		}
	}
}
