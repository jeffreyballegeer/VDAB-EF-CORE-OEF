using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Model.Migrations
{
    /// <inheritdoc />
    public partial class seeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Klanten",
                columns: new[] { "KlantNr", "Voornaam" },
                values: new object[,]
                {
                    { 1, "Marge" },
                    { 2, "Homer" },
                    { 3, "Lisa" },
                    { 4, "Maggie" },
                    { 5, "Simpson" }
                });

            migrationBuilder.InsertData(
                table: "Rekeningen",
                columns: new[] { "RekeningNr", "KlantNr", "Saldo", "Soort" },
                values: new object[,]
                {
                    { "123-4567890-02", 1, 1000m, "Z" },
                    { "234-5678901-69", 1, 2000m, "S" },
                    { "345-6789012-12", 2, 1000m, "S" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Klanten",
                keyColumn: "KlantNr",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Klanten",
                keyColumn: "KlantNr",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Klanten",
                keyColumn: "KlantNr",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Rekeningen",
                keyColumn: "RekeningNr",
                keyValue: "123-4567890-02");

            migrationBuilder.DeleteData(
                table: "Rekeningen",
                keyColumn: "RekeningNr",
                keyValue: "234-5678901-69");

            migrationBuilder.DeleteData(
                table: "Rekeningen",
                keyColumn: "RekeningNr",
                keyValue: "345-6789012-12");

            migrationBuilder.DeleteData(
                table: "Klanten",
                keyColumn: "KlantNr",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Klanten",
                keyColumn: "KlantNr",
                keyValue: 2);
        }
    }
}
