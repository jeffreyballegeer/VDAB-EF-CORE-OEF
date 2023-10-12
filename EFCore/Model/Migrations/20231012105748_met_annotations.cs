using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    /// <inheritdoc />
    public partial class met_annotations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Wedde",
                table: "Docenten",
                newName: "Maandwedde");

            migrationBuilder.RenameColumn(
                name: "Naam",
                table: "Campussen",
                newName: "CampusNaam");

            migrationBuilder.AlterColumn<string>(
                name: "Voornaam",
                table: "Docenten",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Familienaam",
                table: "Docenten",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "HeeftRijbewijs",
                table: "Docenten",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InDienst",
                table: "Docenten",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LandCode",
                table: "Docenten",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Gemeente",
                table: "Campussen",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Landen",
                columns: table => new
                {
                    LandCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Landen", x => x.LandCode);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Docenten_LandCode",
                table: "Docenten",
                column: "LandCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Docenten_Landen_LandCode",
                table: "Docenten",
                column: "LandCode",
                principalTable: "Landen",
                principalColumn: "LandCode",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Docenten_Landen_LandCode",
                table: "Docenten");

            migrationBuilder.DropTable(
                name: "Landen");

            migrationBuilder.DropIndex(
                name: "IX_Docenten_LandCode",
                table: "Docenten");

            migrationBuilder.DropColumn(
                name: "HeeftRijbewijs",
                table: "Docenten");

            migrationBuilder.DropColumn(
                name: "InDienst",
                table: "Docenten");

            migrationBuilder.DropColumn(
                name: "LandCode",
                table: "Docenten");

            migrationBuilder.RenameColumn(
                name: "Maandwedde",
                table: "Docenten",
                newName: "Wedde");

            migrationBuilder.RenameColumn(
                name: "CampusNaam",
                table: "Campussen",
                newName: "Naam");

            migrationBuilder.AlterColumn<string>(
                name: "Voornaam",
                table: "Docenten",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Familienaam",
                table: "Docenten",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Gemeente",
                table: "Campussen",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }
    }
}
