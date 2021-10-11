using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.Services.Identity.Migrations
{
    public partial class AddIdentityProps2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "lastName",
                table: "AspNetUsers",
                newName: "LastName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "AspNetUsers",
                newName: "lastName");
        }
    }
}
