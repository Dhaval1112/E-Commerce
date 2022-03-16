using Microsoft.EntityFrameworkCore.Migrations;

namespace E_Commerce.Migrations
{
    public partial class AddedOrderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressMobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressArea = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressCity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressState = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressLandmark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductSalerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductCoverImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcuctPrice = table.Column<int>(type: "int", nullable: false),
                    ProductQuantity = table.Column<int>(type: "int", nullable: false),
                    ProductTotalDiscount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
