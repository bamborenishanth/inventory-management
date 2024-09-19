using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Product.Inventory.Api.Migrations
{
    public partial class InitiaMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "product_id_seq",
                startValue: 100000L,
                minValue: 100000L,
                maxValue: 999999L,
                cyclic: true);

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('product_id_seq')"),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: true, defaultValue: 1),
                    StockAvailable = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    Rating = table.Column<decimal>(type: "numeric", nullable: true),
                    Category = table.Column<string>(type: "text", nullable: true),
                    LastTimeUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2024, 9, 19, 15, 36, 40, 139, DateTimeKind.Local).AddTicks(7575))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropSequence(
                name: "product_id_seq");
        }
    }
}
