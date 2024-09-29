using System.ComponentModel.DataAnnotations;

namespace Product.Inventory.Api
{
	public class ProductDto
	{
		[StringLength(100, ErrorMessage = "Name can't be longer than 100 characters")]
		public string Name { get; set; }

		[StringLength(500, ErrorMessage = "Description can't be longer than 500 characters")]
		public string? Description { get; set; }

		[Range(0.01, 10000.00, ErrorMessage = "Price must be between 0.01 and 10,000.00")]
		public decimal? Price { get; set; }

		[Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative")]
		public int? Quantity { get; set; }

		[Range(0, 5, ErrorMessage = "Rating must be between 0 and 5")]
		public decimal? Rating { get; set; }

		[StringLength(50, ErrorMessage = "Category can't be longer than 50 characters")]
		public string? Category { get; set; }
	}

}
