using Microsoft.EntityFrameworkCore.Migrations;

namespace ReversiApp.Migrations
{
    public partial class Highscores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Highscore",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Wins = table.Column<int>(nullable: false),
                    Losses = table.Column<int>(nullable: false),
                    WinLossRatio = table.Column<float>(nullable: false),
                    UserIDId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Highscore", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Highscore_AspNetUsers_UserIDId",
                        column: x => x.UserIDId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Highscore_UserIDId",
                table: "Highscore",
                column: "UserIDId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Highscore");
        }
    }
}
