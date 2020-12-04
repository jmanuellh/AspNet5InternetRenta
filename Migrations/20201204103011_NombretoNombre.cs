using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNet5InternetRenta.Migrations
{
    public partial class NombretoNombre : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "InternetRentas",
                newName: "Nombre");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "InternetRentas",
                newName: "Name");
        }
    }
}
