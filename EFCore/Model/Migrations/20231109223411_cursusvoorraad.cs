using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Model.Migrations
{
    /// <inheritdoc />
    public partial class cursusvoorraad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CursusVoorraden",
                columns: table => new
                {
                    MagazijnNr = table.Column<int>(type: "int", nullable: false),
                    CursusNr = table.Column<int>(type: "int", nullable: false),
                    AantalStuks = table.Column<int>(type: "int", nullable: false),
                    RekNr = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CursusVoorraden", x => new { x.MagazijnNr, x.CursusNr });
                });

            migrationBuilder.InsertData(
                table: "CursusVoorraden",
                columns: new[] { "CursusNr", "MagazijnNr", "AantalStuks", "RekNr" },
                values: new object[,]
                {
                    { 10, 1, 100, 3 },
                    { 20, 1, 200, 12 },
                    { 30, 1, 300, 4 },
                    { 10, 2, 1000, 17 },
                    { 20, 2, 2000, 23 },
                    { 30, 2, 3000, 9 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CursusVoorraden");
        }
    }
}
