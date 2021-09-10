using Microsoft.EntityFrameworkCore.Migrations;

namespace KnifeCompany.DataAccess.Migrations
{
    public partial class changedOrderHeader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "OrderHeaders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeaders_ApplicationUserId",
                table: "OrderHeaders",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHeaders_AspNetUsers_ApplicationUserId",
                table: "OrderHeaders",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeaders_AspNetUsers_ApplicationUserId",
                table: "OrderHeaders");

            migrationBuilder.DropIndex(
                name: "IX_OrderHeaders_ApplicationUserId",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "OrderHeaders");
        }
    }
}
