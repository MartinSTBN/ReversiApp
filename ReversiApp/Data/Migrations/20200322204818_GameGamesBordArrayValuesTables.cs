using Microsoft.EntityFrameworkCore.Migrations;

namespace ReversiApp.Data.Migrations
{
    public partial class GameGamesBordArrayValuesTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameID",
                table: "Speler",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    GameID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Omschrijving = table.Column<string>(nullable: true),
                    Token = table.Column<string>(nullable: true),
                    AandeBeurt = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.GameID);
                });

            migrationBuilder.CreateTable(
                name: "BordArrayValues",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArrayIndexX = table.Column<int>(nullable: false),
                    ArrayIndexY = table.Column<int>(nullable: false),
                    Value = table.Column<int>(nullable: false),
                    GameID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BordArrayValues", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BordArrayValues_Game_GameID",
                        column: x => x.GameID,
                        principalTable: "Game",
                        principalColumn: "GameID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Games_Game_GameID",
                        column: x => x.GameID,
                        principalTable: "Game",
                        principalColumn: "GameID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Speler_GameID",
                table: "Speler",
                column: "GameID");

            migrationBuilder.CreateIndex(
                name: "IX_BordArrayValues_GameID",
                table: "BordArrayValues",
                column: "GameID");

            migrationBuilder.CreateIndex(
                name: "IX_Games_GameID",
                table: "Games",
                column: "GameID");

            migrationBuilder.AddForeignKey(
                name: "FK_Speler_Game_GameID",
                table: "Speler",
                column: "GameID",
                principalTable: "Game",
                principalColumn: "GameID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Speler_Game_GameID",
                table: "Speler");

            migrationBuilder.DropTable(
                name: "BordArrayValues");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Game");

            migrationBuilder.DropIndex(
                name: "IX_Speler_GameID",
                table: "Speler");

            migrationBuilder.DropColumn(
                name: "GameID",
                table: "Speler");
        }
    }
}
