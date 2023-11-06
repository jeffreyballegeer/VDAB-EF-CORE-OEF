using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    /// <inheritdoc />
    public partial class TPT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TPT",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TPT", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TPTKlassikaal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Van = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tot = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TPTKlassikaal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TPTKlassikaal_TPT_Id",
                        column: x => x.Id,
                        principalTable: "TPT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TPTZelfstudie",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    AantalDagen = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TPTZelfstudie", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TPTZelfstudie_TPT_Id",
                        column: x => x.Id,
                        principalTable: "TPT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TPTKlassikaal");

            migrationBuilder.DropTable(
                name: "TPTZelfstudie");

            migrationBuilder.DropTable(
                name: "TPT");
        }
    }
}
