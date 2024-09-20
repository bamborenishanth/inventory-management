using System.Collections.Generic;
using System.Threading.Tasks;

namespace Product.Inventory.Api
{
	public interface IProductRepository
	{
		Task<List<Product>> GetAllAsync();
		Task<Product> GetByIdAsync(int id);
		Task<bool> AddAsync(Product product);
		Task<Product> UpdateAsync(int productId, Product product);
		Task<bool> DeleteByIdAsync(int id);
	}
}
