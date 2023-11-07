using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    /// <inheritdoc />
    public partial class metoversteid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Werknemers_Werknemers_OversteWerknemerId",
                table: "Werknemers");

            migrationBuilder.RenameColumn(
                name: "OversteWerknemerId",
                table: "Werknemers",
                newName: "OversteId");

            migrationBuilder.RenameIndex(
                name: "IX_Werknemers_OversteWerknemerId",
                table: "Werknemers",
                newName: "IX_Werknemers_OversteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Werknemers_Werknemers_OversteId",
                table: "Werknemers",
                column: "OversteId",
                principalTable: "Werknemers",
                principalColumn: "WerknemerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Werknemers_Werknemers_OversteId",
                table: "Werknemers");

            migrationBuilder.RenameColumn(
                name: "OversteId",
                table: "Werknemers",
                newName: "OversteWerknemerId");

            migrationBuilder.RenameIndex(
                name: "IX_Werknemers_OversteId",
                table: "Werknemers",
                newName: "IX_Werknemers_OversteWerknemerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Werknemers_Werknemers_OversteWerknemerId",
                table: "Werknemers",
                column: "OversteWerknemerId",
                principalTable: "Werknemers",
                principalColumn: "WerknemerId");
        }
    }
}
