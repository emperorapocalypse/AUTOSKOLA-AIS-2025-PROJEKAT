using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autoskola.DAL.Migrations
{
    /// <inheritdoc />
    public partial class DodajProfilnuSlikuKandidatu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "DatumUpisa",
                table: "Kandidati",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "ProfilnaSlika",
                table: "Kandidati",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilnaSlika",
                table: "Kandidati");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DatumUpisa",
                table: "Kandidati",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }
    }
}
