﻿using System.ComponentModel.DataAnnotations;

namespace Product.Inventory.Api
{
	public class Product
	{
		public int ProductId { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }
		public decimal? Price { get; set; }
		public int? Quantity { get; set; }
		public bool? StockAvailable { get; set; }
		public decimal? Rating { get; set; }
		public string? Category { get; set; }
		public DateTime LastTimeUpdated { get; set; } = DateTime.Now;
	}

}
