using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    /// <inheritdoc />
    public partial class tpc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "TPCCursusSequence");

            migrationBuilder.CreateTable(
                name: "TPCKlassikaal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [TPCCursusSequence]"),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Van = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tot = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TPCKlassikaal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TPCZelfstudie",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [TPCCursusSequence]"),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AantalDagen = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TPCZelfstudie", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TPCKlassikaal");

            migrationBuilder.DropTable(
                name: "TPCZelfstudie");

            migrationBuilder.DropSequence(
                name: "TPCCursusSequence");
        }
    }
}
