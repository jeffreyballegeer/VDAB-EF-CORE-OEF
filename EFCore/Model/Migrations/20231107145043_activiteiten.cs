using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    /// <inheritdoc />
    public partial class activiteiten : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activiteiten",
                columns: table => new
                {
                    ActiviteitId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activiteiten", x => x.ActiviteitId);
                });

            migrationBuilder.CreateTable(
                name: "DocentenActiviteiten",
                columns: table => new
                {
                    DocentId = table.Column<int>(type: "int", nullable: false),
                    ActiviteitId = table.Column<int>(type: "int", nullable: false),
                    AantalUren = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocentenActiviteiten", x => new { x.DocentId, x.ActiviteitId });
                    table.ForeignKey(
                        name: "FK_DocentenActiviteiten_Activiteiten_ActiviteitId",
                        column: x => x.ActiviteitId,
                        principalTable: "Activiteiten",
                        principalColumn: "ActiviteitId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocentenActiviteiten_Docenten_DocentId",
                        column: x => x.DocentId,
                        principalTable: "Docenten",
                        principalColumn: "DocentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocentenActiviteiten_ActiviteitId",
                table: "DocentenActiviteiten",
                column: "ActiviteitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocentenActiviteiten");

            migrationBuilder.DropTable(
                name: "Activiteiten");
        }
    }
}
