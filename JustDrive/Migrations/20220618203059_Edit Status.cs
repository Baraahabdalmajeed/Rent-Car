using Microsoft.EntityFrameworkCore.Migrations;

namespace JustDrive.Migrations
{
    public partial class EditStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Reserved",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Reserved",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
