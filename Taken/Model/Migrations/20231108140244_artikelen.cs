using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    /// <inheritdoc />
    public partial class artikelen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artikelgroepen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artikelgroepen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Artikels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArtikelgroepId = table.Column<int>(type: "int", nullable: false),
                    ArtikelType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Houdbaarheid = table.Column<int>(type: "int", nullable: true),
                    Garantie = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artikels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Artikels_Artikelgroepen_ArtikelgroepId",
                        column: x => x.ArtikelgroepId,
                        principalTable: "Artikelgroepen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Artikels_ArtikelgroepId",
                table: "Artikels",
                column: "ArtikelgroepId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Artikels");

            migrationBuilder.DropTable(
                name: "Artikelgroepen");
        }
    }
}
