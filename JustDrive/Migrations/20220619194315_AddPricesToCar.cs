using Microsoft.EntityFrameworkCore.Migrations;

namespace JustDrive.Migrations
{
    public partial class AddPricesToCar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Car");

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerDay",
                table: "Car",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerHour",
                table: "Car",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerMinutes",
                table: "Car",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricePerDay",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "PricePerHour",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "PricePerMinutes",
                table: "Car");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Car",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
