using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SubastaApi.Migrations
{
    /// <inheritdoc />
    public partial class addDecremento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pagos_StatusPagos_CveStuatusPago",
                table: "Pagos");

            migrationBuilder.RenameColumn(
                name: "CveStuatusPago",
                table: "Pagos",
                newName: "CveStatusPago");

            migrationBuilder.RenameIndex(
                name: "IX_Pagos_CveStuatusPago",
                table: "Pagos",
                newName: "IX_Pagos_CveStatusPago");

            migrationBuilder.AddColumn<decimal>(
                name: "Decremento",
                table: "Subastas",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "IntervaloMinutos",
                table: "Subastas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Pagos_StatusPagos_CveStatusPago",
                table: "Pagos",
                column: "CveStatusPago",
                principalTable: "StatusPagos",
                principalColumn: "IdStatusPago",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pagos_StatusPagos_CveStatusPago",
                table: "Pagos");

            migrationBuilder.DropColumn(
                name: "Decremento",
                table: "Subastas");

            migrationBuilder.DropColumn(
                name: "IntervaloMinutos",
                table: "Subastas");

            migrationBuilder.RenameColumn(
                name: "CveStatusPago",
                table: "Pagos",
                newName: "CveStuatusPago");

            migrationBuilder.RenameIndex(
                name: "IX_Pagos_CveStatusPago",
                table: "Pagos",
                newName: "IX_Pagos_CveStuatusPago");

            migrationBuilder.AddForeignKey(
                name: "FK_Pagos_StatusPagos_CveStuatusPago",
                table: "Pagos",
                column: "CveStuatusPago",
                principalTable: "StatusPagos",
                principalColumn: "IdStatusPago",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
