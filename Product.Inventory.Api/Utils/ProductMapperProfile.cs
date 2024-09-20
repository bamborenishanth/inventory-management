using AutoMapper;

namespace Product.Inventory.Api
{
	public class ProductMapperProfile : Profile
	{
		public ProductMapperProfile()
		{
			CreateMap<ProductDto, Product>();
			CreateMap<Product, ProductDto>();
		}
	}
}
