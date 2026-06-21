using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SubastaApi.Migrations
{
    /// <inheritdoc />
    public partial class FixCalificacionRelacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Calificaciones_CveSubasta",
                table: "Calificaciones");

            migrationBuilder.CreateIndex(
                name: "IX_Calificaciones_CveSubasta",
                table: "Calificaciones",
                column: "CveSubasta");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Calificaciones_CveSubasta",
                table: "Calificaciones");

            migrationBuilder.CreateIndex(
                name: "IX_Calificaciones_CveSubasta",
                table: "Calificaciones",
                column: "CveSubasta",
                unique: true);
        }
    }
}
