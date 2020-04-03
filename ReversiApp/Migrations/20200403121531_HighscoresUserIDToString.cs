using Microsoft.EntityFrameworkCore.Migrations;

namespace ReversiApp.Migrations
{
    public partial class HighscoresUserIDToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Highscore_AspNetUsers_UserIDId",
                table: "Highscore");

            migrationBuilder.DropIndex(
                name: "IX_Highscore_UserIDId",
                table: "Highscore");

            migrationBuilder.DropColumn(
                name: "UserIDId",
                table: "Highscore");

            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "Highscore",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Highscore");

            migrationBuilder.AddColumn<string>(
                name: "UserIDId",
                table: "Highscore",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Highscore_UserIDId",
                table: "Highscore",
                column: "UserIDId");

            migrationBuilder.AddForeignKey(
                name: "FK_Highscore_AspNetUsers_UserIDId",
                table: "Highscore",
                column: "UserIDId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
