using Microsoft.EntityFrameworkCore.Migrations;

namespace JustDrive.Migrations
{
    public partial class ReserveType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReserveType",
                table: "Reserved",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReserveType",
                table: "Reserved");
        }
    }
}
