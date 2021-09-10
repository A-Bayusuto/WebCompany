using Microsoft.EntityFrameworkCore.Migrations;

namespace KnifeCompany.DataAccess.Migrations
{
    public partial class changedShoppingCartVar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "ShoppingCarts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationId",
                table: "ShoppingCarts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
