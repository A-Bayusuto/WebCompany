using Microsoft.EntityFrameworkCore.Migrations;

namespace KnifeCompany.DataAccess.Migrations
{
    public partial class AddStoredProcForProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROC usp_GetProducts
                                    AS 
                                    BEGIN 
                                     SELECT * FROM   dbo.Products 
                                    END");

            migrationBuilder.Sql(@"CREATE PROC usp_GetProduct
                                    @Id int 
                                    AS 
                                    BEGIN 
                                     SELECT * FROM   dbo.Products  WHERE  (Id = @Id) 
                                    END ");

            migrationBuilder.Sql(@"CREATE PROC usp_UpdateProduct
	                                @Id int,
	                                @Name varchar(200),
                                    @Price float,
                                    @Status bit,
                                    @Description varchar(200),
                                    @Picture varchar(200),
                                    @Section varchar(200)
                                    AS 
                                    BEGIN 
                                     UPDATE dbo.Products
                                     SET  Name = @Name, Price = @Price, Status = @Status, Description = @Description, Picture = @Picture, Section = @Section
                                     WHERE  Id = @Id
                                    END");

            migrationBuilder.Sql(@"CREATE PROC usp_DeleteProduct
	                                @Id int
                                    AS 
                                    BEGIN 
                                     DELETE FROM dbo.Products
                                     WHERE  Id = @Id
                                    END");

            migrationBuilder.Sql(@"CREATE PROC usp_CreateProduct
                                   @Name varchar(100),
                                   @Price float,
                                   @Status bit,
                                   @Section varchar(200),
                                   @Description varchar(200),
                                   @Picture varchar(200)
                                   AS 
                                   BEGIN 
                                    INSERT INTO dbo.Products(Name, Price, Status, Section, Description, Picture)
                                    VALUES (@Name, @Price, @Status, @Section, @Description, @Picture)
                                   END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE usp_GetProducts");
            migrationBuilder.Sql(@"DROP PROCEDURE usp_GetProduct");
            migrationBuilder.Sql(@"DROP PROCEDURE usp_UpdateProduct");
            migrationBuilder.Sql(@"DROP PROCEDURE usp_DeleteProduct");
            migrationBuilder.Sql(@"DROP PROCEDURE usp_CreateProduct");
        }
    }
}
