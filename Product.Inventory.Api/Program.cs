using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Net;

namespace Product.Inventory.Api
{
	public class Program
	{
		private static ILogger<Program> _logger;

		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			builder.Logging.ClearProviders();
			builder.Logging.AddConsole();

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddDbContext<ProductContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("InventoryDatabase")));
			builder.Services.AddScoped<IProductService, ProductService>();
			builder.Services.AddScoped<IProductRepository, ProductRepository>();
			builder.Services.AddAutoMapper(typeof(Program));
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
			using ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
			_logger = loggerFactory.CreateLogger<Program>();

			using (var scope = app.Services.CreateScope())
			{
				var dbContext = scope.ServiceProvider.GetRequiredService<ProductContext>();
				// Apply migrations
				try
				{
					dbContext.Database.Migrate();
					_logger.LogInformation("Database Migration Successful");
				}
				catch (Exception ex)
				{
					string message = $"Error while migrating the database, {ex.Message}";
					_logger.LogInformation(message);
				}
				// Seed data
				SeedInitialData(dbContext);
			}
		}

		private static void SeedInitialData(ProductContext dbContext)
		{
			try
			{
				if (!dbContext.Products.Any())
				{
					dbContext.Products.Add(new Product
					{
						Name = "T-Shirt",
						Description = "This is a solid T-Shirt.",
						Price = 9.99m,
						Quantity = 100,
						Category = "Apparel",
						Rating = 4.0m,
					});
					dbContext.SaveChanges();
					_logger.LogInformation("Database seeded with initial entry.");
					return;
				}

				_logger.LogInformation("Database is already seeded.");
			}
			catch (Exception ex)
			{
				string message = $"Error while seeding the database, {ex.Message}";
				_logger.LogError(message);
			}
		}
	}
}
