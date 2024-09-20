using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Product.Inventory.Api.Migrations
{
    public partial class fourthmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StockAvailable",
                table: "Products");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "numeric(10,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldNullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Product_Quantity_NonNegative",
                table: "Products",
                sql: "Quantity >= 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Product_Quantity_NonNegative",
                table: "Products");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "numeric(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "StockAvailable",
                table: "Products",
                type: "boolean",
                nullable: true,
                defaultValue: true);
        }
    }
}
