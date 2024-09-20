using Microsoft.EntityFrameworkCore;

namespace Product.Inventory.Api
{
	public class ProductRepository : IProductRepository, IDisposable
	{
		private readonly ILogger<ProductRepository> _logger;
		private readonly ProductContext _context;
		private bool _disposed = false;

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
				_logger.LogError($"Error occurred while fetching all the products. {ex.Message}");
				return null;
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
				_logger.LogError($"Error occurred fetching the product - {productId}. {ex.Message}");
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
				_logger.LogError($"Error occurred while adding the product. {ex.Message}");
				return false;
			}
		}

		public async Task<Product> UpdateAsync(int productId, Product product)
		{
			try
			{
				_context.Products.Update(product);
				await _context.SaveChangesAsync().ConfigureAwait(false);
				return await GetByIdAsync(productId).ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error occurred while updating the product - {productId}. {ex.Message}");
				return null;
			}
		}

		public async Task<bool> DeleteByIdAsync(int productId)
		{
			try
			{
				Product? product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId).ConfigureAwait(false);
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
				_logger.LogError($"Error occurred while deleting the product - {productId}. {ex.Message}");
				return false;
			}
		}


		// Dispose pattern is not really required to implement here since ProductContext is injected to DI container and it'll be responsible for disposing this object when required
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
