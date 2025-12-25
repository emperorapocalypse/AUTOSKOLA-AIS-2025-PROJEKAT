using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autoskola.DAL.Migrations
{
    
    public partial class DodatoBodoviUIspit : Migration
    {
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Bodovi",
                table: "Ispiti",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bodovi",
                table: "Ispiti");
        }
    }
}
