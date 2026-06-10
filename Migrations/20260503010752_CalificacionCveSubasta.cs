using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SubastaApi.Migrations
{
    /// <inheritdoc />
    public partial class CalificacionCveSubasta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Calificaciones_Subastas_SubastaRefIdSubasta",
                table: "Calificaciones");

            migrationBuilder.DropIndex(
                name: "IX_Calificaciones_SubastaRefIdSubasta",
                table: "Calificaciones");

            migrationBuilder.DropColumn(
                name: "SubastaRefIdSubasta",
                table: "Calificaciones");

            migrationBuilder.CreateIndex(
                name: "IX_Calificaciones_CveSubasta",
                table: "Calificaciones",
                column: "CveSubasta");

            migrationBuilder.AddForeignKey(
                name: "FK_Calificaciones_Subastas_CveSubasta",
                table: "Calificaciones",
                column: "CveSubasta",
                principalTable: "Subastas",
                principalColumn: "IdSubasta",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Calificaciones_Subastas_CveSubasta",
                table: "Calificaciones");

            migrationBuilder.DropIndex(
                name: "IX_Calificaciones_CveSubasta",
                table: "Calificaciones");

            migrationBuilder.AddColumn<int>(
                name: "SubastaRefIdSubasta",
                table: "Calificaciones",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Calificaciones_SubastaRefIdSubasta",
                table: "Calificaciones",
                column: "SubastaRefIdSubasta");

            migrationBuilder.AddForeignKey(
                name: "FK_Calificaciones_Subastas_SubastaRefIdSubasta",
                table: "Calificaciones",
                column: "SubastaRefIdSubasta",
                principalTable: "Subastas",
                principalColumn: "IdSubasta");
        }
    }
}
