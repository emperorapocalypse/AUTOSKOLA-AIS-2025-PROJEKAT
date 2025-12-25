using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autoskola.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Instruktori",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    JMBG = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrojLicence = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    GodineIskustva = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instruktori", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kandidati",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    JMBG = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatumUpisa = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kandidati", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vozila",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Marka = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Registracija = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    GodinaProizvodnje = table.Column<int>(type: "int", nullable: true),
                    TipGoriva = table.Column<int>(type: "int", nullable: false),
                    VoznoStanje = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vozila", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ispiti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VremePocetka = table.Column<TimeSpan>(type: "time", nullable: false),
                    TipIspita = table.Column<int>(type: "int", nullable: false),
                    InstruktorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ispiti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ispiti_Instruktori_InstruktorId",
                        column: x => x.InstruktorId,
                        principalTable: "Instruktori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Casovi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipCasa = table.Column<int>(type: "int", nullable: false),
                    BrojCasa = table.Column<int>(type: "int", maxLength: 10, nullable: false),
                    Datum = table.Column<DateOnly>(type: "date", nullable: true),
                    InstruktorId = table.Column<int>(type: "int", nullable: true),
                    VoziloId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Casovi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Casovi_Instruktori_InstruktorId",
                        column: x => x.InstruktorId,
                        principalTable: "Instruktori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Casovi_Vozila_VoziloId",
                        column: x => x.VoziloId,
                        principalTable: "Vozila",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "IspitVozila",
                columns: table => new
                {
                    IspitId = table.Column<int>(type: "int", nullable: false),
                    VoziloId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IspitVozila", x => new { x.IspitId, x.VoziloId });
                    table.ForeignKey(
                        name: "FK_IspitVozila_Ispiti_IspitId",
                        column: x => x.IspitId,
                        principalTable: "Ispiti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IspitVozila_Vozila_VoziloId",
                        column: x => x.VoziloId,
                        principalTable: "Vozila",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KandidatIspiti",
                columns: table => new
                {
                    KandidatId = table.Column<int>(type: "int", nullable: false),
                    IspitId = table.Column<int>(type: "int", nullable: false),
                    Polozio = table.Column<bool>(type: "bit", nullable: false),
                    BrojBodova = table.Column<int>(type: "int", nullable: false),
                    Napomena = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KandidatIspiti", x => new { x.KandidatId, x.IspitId });
                    table.ForeignKey(
                        name: "FK_KandidatIspiti_Ispiti_IspitId",
                        column: x => x.IspitId,
                        principalTable: "Ispiti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KandidatIspiti_Kandidati_KandidatId",
                        column: x => x.KandidatId,
                        principalTable: "Kandidati",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KandidatCasovi",
                columns: table => new
                {
                    KandidatId = table.Column<int>(type: "int", nullable: false),
                    CasId = table.Column<int>(type: "int", nullable: false),
                    Prisustvovao = table.Column<bool>(type: "bit", nullable: false),
                    Napomena = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KandidatCasovi", x => new { x.KandidatId, x.CasId });
                    table.ForeignKey(
                        name: "FK_KandidatCasovi_Casovi_CasId",
                        column: x => x.CasId,
                        principalTable: "Casovi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KandidatCasovi_Kandidati_KandidatId",
                        column: x => x.KandidatId,
                        principalTable: "Kandidati",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Casovi_InstruktorId",
                table: "Casovi",
                column: "InstruktorId");

            migrationBuilder.CreateIndex(
                name: "IX_Casovi_VoziloId",
                table: "Casovi",
                column: "VoziloId");

            migrationBuilder.CreateIndex(
                name: "IX_Ispiti_InstruktorId",
                table: "Ispiti",
                column: "InstruktorId");

            migrationBuilder.CreateIndex(
                name: "IX_IspitVozila_VoziloId",
                table: "IspitVozila",
                column: "VoziloId");

            migrationBuilder.CreateIndex(
                name: "IX_KandidatCasovi_CasId",
                table: "KandidatCasovi",
                column: "CasId");

            migrationBuilder.CreateIndex(
                name: "IX_KandidatIspiti_IspitId",
                table: "KandidatIspiti",
                column: "IspitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IspitVozila");

            migrationBuilder.DropTable(
                name: "KandidatCasovi");

            migrationBuilder.DropTable(
                name: "KandidatIspiti");

            migrationBuilder.DropTable(
                name: "Casovi");

            migrationBuilder.DropTable(
                name: "Ispiti");

            migrationBuilder.DropTable(
                name: "Kandidati");

            migrationBuilder.DropTable(
                name: "Vozila");

            migrationBuilder.DropTable(
                name: "Instruktori");
        }
    }
}
