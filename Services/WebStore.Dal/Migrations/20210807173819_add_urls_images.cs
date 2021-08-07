using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebStore.Dal.Migrations
{
    public partial class add_urls_images : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImageUrls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageUrls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImageUrlProduct",
                columns: table => new
                {
                    ImageUrlsId = table.Column<int>(type: "int", nullable: false),
                    ProductsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageUrlProduct", x => new { x.ImageUrlsId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_ImageUrlProduct_ImageUrls_ImageUrlsId",
                        column: x => x.ImageUrlsId,
                        principalTable: "ImageUrls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImageUrlProduct_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageUrlProduct_ProductsId",
                table: "ImageUrlProduct",
                column: "ProductsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageUrlProduct");

            migrationBuilder.DropTable(
                name: "ImageUrls");
        }
    }
}
