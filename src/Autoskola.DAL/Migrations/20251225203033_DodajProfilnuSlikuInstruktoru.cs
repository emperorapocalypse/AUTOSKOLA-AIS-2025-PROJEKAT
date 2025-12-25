using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autoskola.DAL.Migrations
{
    /// <inheritdoc />
    public partial class DodajProfilnuSlikuInstruktoru : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DatumUpisa",
                table: "Kandidati",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<string>(
                name: "ProfilnaSlika",
                table: "Instruktori",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilnaSlika",
                table: "Instruktori");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DatumUpisa",
                table: "Kandidati",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
