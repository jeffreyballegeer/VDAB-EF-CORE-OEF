using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Brouwers",
            //    columns: table => new
            //    {
            //        BrouwerNr = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        BrNaam = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        Adres = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        PostCode = table.Column<short>(type: "smallint", nullable: false),
            //        Gemeente = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        Omzet = table.Column<int>(type: "int", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("Brouwers$PrimaryKey", x => x.BrouwerNr);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Soorten",
            //    columns: table => new
            //    {
            //        SoortNr = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        SoortNaam = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("Soorten$PrimaryKey", x => x.SoortNr);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Bieren",
            //    columns: table => new
            //    {
            //        BierNr = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Naam = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            //        BrouwerNr = table.Column<int>(type: "int", nullable: false),
            //        SoortNr = table.Column<int>(type: "int", nullable: false),
            //        Alcohol = table.Column<float>(type: "real", nullable: true),
            //        SSMA_TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("Bieren$PrimaryKey", x => x.BierNr);
            //        table.ForeignKey(
            //            name: "Bieren$BrouwersBieren",
            //            column: x => x.BrouwerNr,
            //            principalTable: "Brouwers",
            //            principalColumn: "BrouwerNr");
            //        table.ForeignKey(
            //            name: "Bieren$SoortenBieren",
            //            column: x => x.SoortNr,
            //            principalTable: "Soorten",
            //            principalColumn: "SoortNr");
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Bieren_BrouwerNr",
            //    table: "Bieren",
            //    column: "BrouwerNr");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Bieren_SoortNr",
            //    table: "Bieren",
            //    column: "SoortNr");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bieren");

            migrationBuilder.DropTable(
                name: "Brouwers");

            migrationBuilder.DropTable(
                name: "Soorten");
        }
    }
}
