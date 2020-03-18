using Microsoft.EntityFrameworkCore.Migrations;

namespace CoffeeSlotMachine.Persistence.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DonationCents",
                table: "Orders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReturnCents",
                table: "Orders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ThrownInCents",
                table: "Orders",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DonationCents",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ReturnCents",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ThrownInCents",
                table: "Orders");
        }
    }
}
