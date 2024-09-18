using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Net;

namespace Product.Inventory.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddDbContext<ProductContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("InventoryDatabase")));
			builder.Services.AddScoped<IProductService, ProductService>();
			var app = builder.Build();

			ApplyMigrationsAndSeedData(app);

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();
			app.MapControllers();
			app.Run();
		}

		private static void ApplyMigrationsAndSeedData(WebApplication app)
		{
			using (var scope = app.Services.CreateScope())
			{
				var dbContext = scope.ServiceProvider.GetRequiredService<ProductContext>();

				// Apply migrations
				dbContext.Database.Migrate();

				// Seed data
				SeedInitialData(dbContext);
			}
		}

		private static void SeedInitialData(ProductContext dbContext)
		{
			if (!dbContext.Products.Any())
			{
				dbContext.Products.Add(new Product
				{
					Name = "T-Shirt",
					Description = "This is a solid T-Shirt.",
					Price = 9.99m,
					Quantity = 100,
					StockAvailable = true,
					Category = "Apparel",
					Rating = 4.0m,
				});
				dbContext.SaveChanges();
			}
		}


	}
}
