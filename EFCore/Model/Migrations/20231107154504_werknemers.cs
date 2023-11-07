using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    /// <inheritdoc />
    public partial class werknemers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Werknemers",
                columns: table => new
                {
                    WerknemerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Voornaam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Familienaam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OversteWerknemerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Werknemers", x => x.WerknemerId);
                    table.ForeignKey(
                        name: "FK_Werknemers_Werknemers_OversteWerknemerId",
                        column: x => x.OversteWerknemerId,
                        principalTable: "Werknemers",
                        principalColumn: "WerknemerId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Werknemers_OversteWerknemerId",
                table: "Werknemers",
                column: "OversteWerknemerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Werknemers");
        }
    }
}
