using Microsoft.EntityFrameworkCore.Migrations;

namespace E_Commerce.Migrations
{
    public partial class AddedStatusOfAddToCartIntoCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CartStatus",
                table: "Carts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CartStatus",
                table: "Carts");
        }
    }
}
