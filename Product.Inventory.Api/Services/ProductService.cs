using AutoMapper;

namespace Product.Inventory.Api
{
	public class ProductService : IProductService
	{
		private readonly ILogger<ProductService> _logger;
		private readonly IProductRepository _productRepository;
		private readonly IMapper _mapper;

		public ProductService(ILogger<ProductService> logger, IProductRepository productRepository, IMapper mapper)
		{
			_logger = logger;
			_productRepository = productRepository;
			_mapper = mapper;
		}

		public async Task<List<Product>> GetAllProducts()
		{
			List<Product> products = await _productRepository.GetAllAsync().ConfigureAwait(false);

			return products;
		}

		public async Task<Product> GetProductById(int productId)
		{
			return await _productRepository.GetByIdAsync(productId);
		}

		public async Task<bool> AddProduct(ProductDto product)
		{
			Product newProduct = new();
			_mapper.Map(product, newProduct);
			return await _productRepository.AddAsync(newProduct);
		}

		public async Task<Product> UpdateProduct(int productId, ProductDto productDto)
		{
			Product existingProduct = await GetProductById(productId).ConfigureAwait(false);
			if (existingProduct != null)
			{
				_mapper.Map(productDto, existingProduct);
				Product product = await _productRepository.UpdateAsync(productId, existingProduct).ConfigureAwait(false);
				return product;
			}
			return null;
		}

		public async Task<Product> UpdateStock(int productId, int quantity, bool addStock)
		{
			Product product = await GetProductById(productId).ConfigureAwait(false);
			if (product != null)
			{
				if (addStock == false && product.Quantity - quantity < 0)
				{
					_logger.LogError("Product quantity cannot be less than 0");
					return null;
				}
				product.Quantity = addStock ? product.Quantity + quantity : product.Quantity - quantity;
				Product updatedProduct = await _productRepository.UpdateAsync(productId, product).ConfigureAwait(false);
				return updatedProduct;
			}
			return null;
		}

		public async Task<bool> DeleteProduct(int productId)
		{
			return await _productRepository.DeleteByIdAsync(productId);
		}
	}
}
