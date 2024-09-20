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
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
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
				_logger.LogError(ex, $"Error occurred fetching the product with ID: {productId}.");
				return null;
			}
		}

		public async Task<bool> AddAsync(Product product)
		{
			if (product == null)
			{
				throw new ArgumentNullException(nameof(product));
			}

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
			if (product == null)
			{
				throw new ArgumentNullException(nameof(product));
			}

			try
			{
				_context.Products.Update(product);
				await _context.SaveChangesAsync().ConfigureAwait(false);
				return await GetByIdAsync(productId).ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error occurred while updating the product with ID: {productId}.");
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
				_logger.LogError(ex, $"Error occurred while deleting the product with ID: {productId}.");
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
