using Microsoft.EntityFrameworkCore.Migrations;

namespace JustDrive.Migrations
{
    public partial class img : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Car_CarId",
                table: "Image");

            migrationBuilder.AlterColumn<int>(
                name: "CarId",
                table: "Image",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Car_CarId",
                table: "Image",
                column: "CarId",
                principalTable: "Car",
                principalColumn: "CarId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Car_CarId",
                table: "Image");

            migrationBuilder.AlterColumn<int>(
                name: "CarId",
                table: "Image",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Car_CarId",
                table: "Image",
                column: "CarId",
                principalTable: "Car",
                principalColumn: "CarId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
