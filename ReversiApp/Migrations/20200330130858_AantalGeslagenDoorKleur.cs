using Microsoft.EntityFrameworkCore.Migrations;

namespace ReversiApp.Migrations
{
    public partial class AantalGeslagenDoorKleur : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "aantalGeslagenDoorWit",
                table: "Game",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "aantalGeslagenDoorZwart",
                table: "Game",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "aantalGeslagenDoorWit",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "aantalGeslagenDoorZwart",
                table: "Game");
        }
    }
}
