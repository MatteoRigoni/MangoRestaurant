using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.Services.Products.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "Category", "CreateAt", "Description", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Appetizer", new DateTime(2021, 10, 9, 18, 7, 30, 542, DateTimeKind.Local).AddTicks(4497), "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.", "https://res.cloudinary.com/dz57kxe4m/image/upload/v1633794000/13_jzagez.jpg", "Samosa", 15.0 },
                    { 2, "Appetizer", new DateTime(2021, 10, 9, 18, 7, 30, 542, DateTimeKind.Local).AddTicks(4674), "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.", "https://res.cloudinary.com/dz57kxe4m/image/upload/v1633794000/11_ordhyz.jpg", "Paneer Tikka", 13.99 },
                    { 3, "Dessert", new DateTime(2021, 10, 9, 18, 7, 30, 542, DateTimeKind.Local).AddTicks(4717), "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.", "https://res.cloudinary.com/dz57kxe4m/image/upload/v1633794000/14_wrni0e.jpg", "Sweet Pie", 10.99 },
                    { 4, "Entree", new DateTime(2021, 10, 9, 18, 7, 30, 542, DateTimeKind.Local).AddTicks(4752), "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.", "https://res.cloudinary.com/dz57kxe4m/image/upload/v1633794000/12_hpkj3u.jpgk", "Pav Bhaji", 15.0 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "Products");
        }
    }
}
