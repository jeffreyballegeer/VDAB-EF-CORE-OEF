using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    /// <inheritdoc />
    public partial class new_annotations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Klanten",
                columns: table => new
                {
                    KlantNr = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Voornaam = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klanten", x => x.KlantNr);
                });

            migrationBuilder.CreateTable(
                name: "Rekeningen",
                columns: table => new
                {
                    RekeningNr = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    KlantNr = table.Column<int>(type: "int", nullable: false),
                    Saldo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Soort = table.Column<string>(type: "nvarchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rekeningen", x => x.RekeningNr);
                    table.ForeignKey(
                        name: "FK_Rekeningen_Klanten_KlantNr",
                        column: x => x.KlantNr,
                        principalTable: "Klanten",
                        principalColumn: "KlantNr",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rekeningen_KlantNr",
                table: "Rekeningen",
                column: "KlantNr");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rekeningen");

            migrationBuilder.DropTable(
                name: "Klanten");
        }
    }
}
