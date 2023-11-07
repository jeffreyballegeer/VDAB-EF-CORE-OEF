using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Model.Migrations
{
    /// <inheritdoc />
    public partial class boekencursussen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Boeken",
                columns: table => new
                {
                    BoekNr = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsbnNr = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Titel = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boeken", x => x.BoekNr);
                });

            migrationBuilder.CreateTable(
                name: "Cursussen",
                columns: table => new
                {
                    CursusNr = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cursussen", x => x.CursusNr);
                });

            migrationBuilder.CreateTable(
                name: "BoekCursus",
                columns: table => new
                {
                    BoekenBoekNr = table.Column<int>(type: "int", nullable: false),
                    CursussenCursusNr = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoekCursus", x => new { x.BoekenBoekNr, x.CursussenCursusNr });
                    table.ForeignKey(
                        name: "FK_BoekCursus_Boeken_BoekenBoekNr",
                        column: x => x.BoekenBoekNr,
                        principalTable: "Boeken",
                        principalColumn: "BoekNr",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoekCursus_Cursussen_CursussenCursusNr",
                        column: x => x.CursussenCursusNr,
                        principalTable: "Cursussen",
                        principalColumn: "CursusNr",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Boeken",
                columns: new[] { "BoekNr", "IsbnNr", "Titel" },
                values: new object[,]
                {
                    { 1, "0-0705918-0-6", "C++ : For Scientists and Engineers" },
                    { 2, "0-0788212-3-1", "C++ : The Complete Reference" },
                    { 3, "1-5659211-6-X", "C++ : The Core Language" },
                    { 4, "0-4448771-8-5", "Relational Database Systems" },
                    { 5, "1-5595851-1-0", "Access from the Ground Up" },
                    { 6, "0-0788212-2-3", "Oracle : A Beginner''s Guide" },
                    { 7, "0-0788209-7-9", "Oracle : The Complete Reference" }
                });

            migrationBuilder.InsertData(
                table: "Cursussen",
                columns: new[] { "CursusNr", "Naam" },
                values: new object[,]
                {
                    { 1, "C++" },
                    { 2, "Access" },
                    { 3, "Oracle" }
                });

            migrationBuilder.InsertData(
                table: "BoekCursus",
                columns: new[] { "BoekenBoekNr", "CursussenCursusNr" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 4, 2 },
                    { 4, 3 },
                    { 5, 2 },
                    { 6, 3 },
                    { 7, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoekCursus_CursussenCursusNr",
                table: "BoekCursus",
                column: "CursussenCursusNr");

            migrationBuilder.CreateIndex(
                name: "IX_Boeken_IsbnNr",
                table: "Boeken",
                column: "IsbnNr",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoekCursus");

            migrationBuilder.DropTable(
                name: "Boeken");

            migrationBuilder.DropTable(
                name: "Cursussen");
        }
    }
}
