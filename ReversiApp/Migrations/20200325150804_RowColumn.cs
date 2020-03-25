using Microsoft.EntityFrameworkCore.Migrations;

namespace ReversiApp.Migrations
{
    public partial class RowColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArrayIndexX",
                table: "BordArrayValues");

            migrationBuilder.DropColumn(
                name: "ArrayIndexY",
                table: "BordArrayValues");

            migrationBuilder.AddColumn<int>(
                name: "Column",
                table: "BordArrayValues",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Row",
                table: "BordArrayValues",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Column",
                table: "BordArrayValues");

            migrationBuilder.DropColumn(
                name: "Row",
                table: "BordArrayValues");

            migrationBuilder.AddColumn<int>(
                name: "ArrayIndexX",
                table: "BordArrayValues",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ArrayIndexY",
                table: "BordArrayValues",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
