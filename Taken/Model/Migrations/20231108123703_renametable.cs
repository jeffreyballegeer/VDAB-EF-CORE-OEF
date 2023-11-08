using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    /// <inheritdoc />
    public partial class renametable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personeelslid_Personeelslid_ManagerNr",
                table: "Personeelslid");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Personeelslid",
                table: "Personeelslid");

            migrationBuilder.RenameTable(
                name: "Personeelslid",
                newName: "Personeelsleden");

            migrationBuilder.RenameIndex(
                name: "IX_Personeelslid_ManagerNr",
                table: "Personeelsleden",
                newName: "IX_Personeelsleden_ManagerNr");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Personeelsleden",
                table: "Personeelsleden",
                column: "PersoneelsNr");

            migrationBuilder.AddForeignKey(
                name: "FK_Personeelsleden_Personeelsleden_ManagerNr",
                table: "Personeelsleden",
                column: "ManagerNr",
                principalTable: "Personeelsleden",
                principalColumn: "PersoneelsNr");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personeelsleden_Personeelsleden_ManagerNr",
                table: "Personeelsleden");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Personeelsleden",
                table: "Personeelsleden");

            migrationBuilder.RenameTable(
                name: "Personeelsleden",
                newName: "Personeelslid");

            migrationBuilder.RenameIndex(
                name: "IX_Personeelsleden_ManagerNr",
                table: "Personeelslid",
                newName: "IX_Personeelslid_ManagerNr");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Personeelslid",
                table: "Personeelslid",
                column: "PersoneelsNr");

            migrationBuilder.AddForeignKey(
                name: "FK_Personeelslid_Personeelslid_ManagerNr",
                table: "Personeelslid",
                column: "ManagerNr",
                principalTable: "Personeelslid",
                principalColumn: "PersoneelsNr");
        }
    }
}
