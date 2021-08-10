using Microsoft.EntityFrameworkCore.Migrations;

namespace KnifeCompany.DataAccess.Migrations
{
    public partial class UpdatePostalVariable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Postal",
                table: "AspNetUsers",
                newName: "PostalCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PostalCode",
                table: "AspNetUsers",
                newName: "Postal");
        }
    }
}
