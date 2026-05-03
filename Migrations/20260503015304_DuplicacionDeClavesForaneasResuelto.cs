using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SubastaApi.Migrations
{
    /// <inheritdoc />
    public partial class DuplicacionDeClavesForaneasResuelto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FotosProducto_Productos_ProductoRefIdProducto",
                table: "FotosProducto");

            migrationBuilder.DropForeignKey(
                name: "FK_Inmuebles_Productos_ProductoRefIdProducto",
                table: "Inmuebles");

            migrationBuilder.DropForeignKey(
                name: "FK_Notificaciones_Ofertas_OfertaRefIdOferta",
                table: "Notificaciones");

            migrationBuilder.DropForeignKey(
                name: "FK_Notificaciones_Subastas_SubastaRefIdSubasta",
                table: "Notificaciones");

            migrationBuilder.DropForeignKey(
                name: "FK_Notificaciones_TiposNotificacion_TipoNotifiacionRefIdTipoNotificacion",
                table: "Notificaciones");

            migrationBuilder.DropForeignKey(
                name: "FK_Notificaciones_Usuarios_UsuarioRefIdUsuario",
                table: "Notificaciones");

            migrationBuilder.DropForeignKey(
                name: "FK_Ofertas_Subastas_SubastaRefIdSubasta",
                table: "Ofertas");

            migrationBuilder.DropForeignKey(
                name: "FK_Ofertas_Usuarios_UsuarioRefIdUsuario",
                table: "Ofertas");

            migrationBuilder.DropForeignKey(
                name: "FK_Pagos_StatusPagos_StatusPagoRefIdStatusPago",
                table: "Pagos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pagos_Subastas_SubastaRefIdSubasta",
                table: "Pagos");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Categorias_CategoriaRefIdCategoria",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Condiciones_CondicionRefIdCondicion",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_StatusProductos_StatusProductoRefIdStatusProducto",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Usuarios_UsuarioRefIdUsuario",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Subastas_Productos_ProductoRefIdProducto",
                table: "Subastas");

            migrationBuilder.DropForeignKey(
                name: "FK_Subastas_StatusSubastas_StatusSubastaRefIdStatusSubasta",
                table: "Subastas");

            migrationBuilder.DropForeignKey(
                name: "FK_Subastas_TiposSubasta_TipoSubastaRefIdTipoSubasta",
                table: "Subastas");

            migrationBuilder.DropForeignKey(
                name: "FK_Subastas_Usuarios_UsuarioGanadorRefIdUsuario",
                table: "Subastas");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_TiposUsuario_TipoUsuarioRefIdTipoUsuario",
                table: "Usuarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehiculos_Productos_ProductoRefIdProducto",
                table: "Vehiculos");

            migrationBuilder.DropIndex(
                name: "IX_Vehiculos_ProductoRefIdProducto",
                table: "Vehiculos");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_TipoUsuarioRefIdTipoUsuario",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Subastas_ProductoRefIdProducto",
                table: "Subastas");

            migrationBuilder.DropIndex(
                name: "IX_Subastas_StatusSubastaRefIdStatusSubasta",
                table: "Subastas");

            migrationBuilder.DropIndex(
                name: "IX_Subastas_TipoSubastaRefIdTipoSubasta",
                table: "Subastas");

            migrationBuilder.DropIndex(
                name: "IX_Subastas_UsuarioGanadorRefIdUsuario",
                table: "Subastas");

            migrationBuilder.DropIndex(
                name: "IX_Productos_CategoriaRefIdCategoria",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_Productos_CondicionRefIdCondicion",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_Productos_StatusProductoRefIdStatusProducto",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_Productos_UsuarioRefIdUsuario",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_Pagos_StatusPagoRefIdStatusPago",
                table: "Pagos");

            migrationBuilder.DropIndex(
                name: "IX_Pagos_SubastaRefIdSubasta",
                table: "Pagos");

            migrationBuilder.DropIndex(
                name: "IX_Ofertas_SubastaRefIdSubasta",
                table: "Ofertas");

            migrationBuilder.DropIndex(
                name: "IX_Ofertas_UsuarioRefIdUsuario",
                table: "Ofertas");

            migrationBuilder.DropIndex(
                name: "IX_Notificaciones_OfertaRefIdOferta",
                table: "Notificaciones");

            migrationBuilder.DropIndex(
                name: "IX_Notificaciones_SubastaRefIdSubasta",
                table: "Notificaciones");

            migrationBuilder.DropIndex(
                name: "IX_Notificaciones_TipoNotifiacionRefIdTipoNotificacion",
                table: "Notificaciones");

            migrationBuilder.DropIndex(
                name: "IX_Notificaciones_UsuarioRefIdUsuario",
                table: "Notificaciones");

            migrationBuilder.DropIndex(
                name: "IX_Inmuebles_ProductoRefIdProducto",
                table: "Inmuebles");

            migrationBuilder.DropIndex(
                name: "IX_FotosProducto_ProductoRefIdProducto",
                table: "FotosProducto");

            migrationBuilder.DropIndex(
                name: "IX_Calificaciones_CveSubasta",
                table: "Calificaciones");

            migrationBuilder.DropColumn(
                name: "ProductoRefIdProducto",
                table: "Vehiculos");

            migrationBuilder.DropColumn(
                name: "TipoUsuarioRefIdTipoUsuario",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "ProductoRefIdProducto",
                table: "Subastas");

            migrationBuilder.DropColumn(
                name: "StatusSubastaRefIdStatusSubasta",
                table: "Subastas");

            migrationBuilder.DropColumn(
                name: "TipoSubastaRefIdTipoSubasta",
                table: "Subastas");

            migrationBuilder.DropColumn(
                name: "UsuarioGanadorRefIdUsuario",
                table: "Subastas");

            migrationBuilder.DropColumn(
                name: "CategoriaRefIdCategoria",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "CondicionRefIdCondicion",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "StatusProductoRefIdStatusProducto",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "UsuarioRefIdUsuario",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "StatusPagoRefIdStatusPago",
                table: "Pagos");

            migrationBuilder.DropColumn(
                name: "SubastaRefIdSubasta",
                table: "Pagos");

            migrationBuilder.DropColumn(
                name: "SubastaRefIdSubasta",
                table: "Ofertas");

            migrationBuilder.DropColumn(
                name: "UsuarioRefIdUsuario",
                table: "Ofertas");

            migrationBuilder.DropColumn(
                name: "OfertaRefIdOferta",
                table: "Notificaciones");

            migrationBuilder.DropColumn(
                name: "SubastaRefIdSubasta",
                table: "Notificaciones");

            migrationBuilder.DropColumn(
                name: "TipoNotifiacionRefIdTipoNotificacion",
                table: "Notificaciones");

            migrationBuilder.DropColumn(
                name: "UsuarioRefIdUsuario",
                table: "Notificaciones");

            migrationBuilder.DropColumn(
                name: "ProductoRefIdProducto",
                table: "Inmuebles");

            migrationBuilder.DropColumn(
                name: "ProductoRefIdProducto",
                table: "FotosProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Vehiculos_CveProducto",
                table: "Vehiculos",
                column: "CveProducto",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_CveTipoUsuario",
                table: "Usuarios",
                column: "CveTipoUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Subastas_CveProducto",
                table: "Subastas",
                column: "CveProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Subastas_CveStatusSubasta",
                table: "Subastas",
                column: "CveStatusSubasta");

            migrationBuilder.CreateIndex(
                name: "IX_Subastas_CveTipoSubasta",
                table: "Subastas",
                column: "CveTipoSubasta");

            migrationBuilder.CreateIndex(
                name: "IX_Subastas_CveUsuarioGanador",
                table: "Subastas",
                column: "CveUsuarioGanador");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_CveCategoria",
                table: "Productos",
                column: "CveCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_CveCondicion",
                table: "Productos",
                column: "CveCondicion");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_CveStatusProducto",
                table: "Productos",
                column: "CveStatusProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_CveUsuario",
                table: "Productos",
                column: "CveUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_CveStuatusPago",
                table: "Pagos",
                column: "CveStuatusPago");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_CveSubasta",
                table: "Pagos",
                column: "CveSubasta",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ofertas_CveSubasta",
                table: "Ofertas",
                column: "CveSubasta");

            migrationBuilder.CreateIndex(
                name: "IX_Ofertas_CveUsuario",
                table: "Ofertas",
                column: "CveUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_CveOferta",
                table: "Notificaciones",
                column: "CveOferta");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_CveSubasta",
                table: "Notificaciones",
                column: "CveSubasta");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_CveTipoNotificacion",
                table: "Notificaciones",
                column: "CveTipoNotificacion");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_CveUsuario",
                table: "Notificaciones",
                column: "CveUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Inmuebles_CveProducto",
                table: "Inmuebles",
                column: "CveProducto",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FotosProducto_CveProducto",
                table: "FotosProducto",
                column: "CveProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Calificaciones_CveSubasta",
                table: "Calificaciones",
                column: "CveSubasta",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FotosProducto_Productos_CveProducto",
                table: "FotosProducto",
                column: "CveProducto",
                principalTable: "Productos",
                principalColumn: "IdProducto",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Inmuebles_Productos_CveProducto",
                table: "Inmuebles",
                column: "CveProducto",
                principalTable: "Productos",
                principalColumn: "IdProducto",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notificaciones_Ofertas_CveOferta",
                table: "Notificaciones",
                column: "CveOferta",
                principalTable: "Ofertas",
                principalColumn: "IdOferta",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notificaciones_Subastas_CveSubasta",
                table: "Notificaciones",
                column: "CveSubasta",
                principalTable: "Subastas",
                principalColumn: "IdSubasta",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notificaciones_TiposNotificacion_CveTipoNotificacion",
                table: "Notificaciones",
                column: "CveTipoNotificacion",
                principalTable: "TiposNotificacion",
                principalColumn: "IdTipoNotificacion",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notificaciones_Usuarios_CveUsuario",
                table: "Notificaciones",
                column: "CveUsuario",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ofertas_Subastas_CveSubasta",
                table: "Ofertas",
                column: "CveSubasta",
                principalTable: "Subastas",
                principalColumn: "IdSubasta",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ofertas_Usuarios_CveUsuario",
                table: "Ofertas",
                column: "CveUsuario",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pagos_StatusPagos_CveStuatusPago",
                table: "Pagos",
                column: "CveStuatusPago",
                principalTable: "StatusPagos",
                principalColumn: "IdStatusPago",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pagos_Subastas_CveSubasta",
                table: "Pagos",
                column: "CveSubasta",
                principalTable: "Subastas",
                principalColumn: "IdSubasta",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Categorias_CveCategoria",
                table: "Productos",
                column: "CveCategoria",
                principalTable: "Categorias",
                principalColumn: "IdCategoria",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Condiciones_CveCondicion",
                table: "Productos",
                column: "CveCondicion",
                principalTable: "Condiciones",
                principalColumn: "IdCondicion",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_StatusProductos_CveStatusProducto",
                table: "Productos",
                column: "CveStatusProducto",
                principalTable: "StatusProductos",
                principalColumn: "IdStatusProducto",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Usuarios_CveUsuario",
                table: "Productos",
                column: "CveUsuario",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subastas_Productos_CveProducto",
                table: "Subastas",
                column: "CveProducto",
                principalTable: "Productos",
                principalColumn: "IdProducto",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subastas_StatusSubastas_CveStatusSubasta",
                table: "Subastas",
                column: "CveStatusSubasta",
                principalTable: "StatusSubastas",
                principalColumn: "IdStatusSubasta",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subastas_TiposSubasta_CveTipoSubasta",
                table: "Subastas",
                column: "CveTipoSubasta",
                principalTable: "TiposSubasta",
                principalColumn: "IdTipoSubasta",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subastas_Usuarios_CveUsuarioGanador",
                table: "Subastas",
                column: "CveUsuarioGanador",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_TiposUsuario_CveTipoUsuario",
                table: "Usuarios",
                column: "CveTipoUsuario",
                principalTable: "TiposUsuario",
                principalColumn: "IdTipoUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehiculos_Productos_CveProducto",
                table: "Vehiculos",
                column: "CveProducto",
                principalTable: "Productos",
                principalColumn: "IdProducto",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FotosProducto_Productos_CveProducto",
                table: "FotosProducto");

            migrationBuilder.DropForeignKey(
                name: "FK_Inmuebles_Productos_CveProducto",
                table: "Inmuebles");

            migrationBuilder.DropForeignKey(
                name: "FK_Notificaciones_Ofertas_CveOferta",
                table: "Notificaciones");

            migrationBuilder.DropForeignKey(
                name: "FK_Notificaciones_Subastas_CveSubasta",
                table: "Notificaciones");

            migrationBuilder.DropForeignKey(
                name: "FK_Notificaciones_TiposNotificacion_CveTipoNotificacion",
                table: "Notificaciones");

            migrationBuilder.DropForeignKey(
                name: "FK_Notificaciones_Usuarios_CveUsuario",
                table: "Notificaciones");

            migrationBuilder.DropForeignKey(
                name: "FK_Ofertas_Subastas_CveSubasta",
                table: "Ofertas");

            migrationBuilder.DropForeignKey(
                name: "FK_Ofertas_Usuarios_CveUsuario",
                table: "Ofertas");

            migrationBuilder.DropForeignKey(
                name: "FK_Pagos_StatusPagos_CveStuatusPago",
                table: "Pagos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pagos_Subastas_CveSubasta",
                table: "Pagos");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Categorias_CveCategoria",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Condiciones_CveCondicion",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_StatusProductos_CveStatusProducto",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Usuarios_CveUsuario",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Subastas_Productos_CveProducto",
                table: "Subastas");

            migrationBuilder.DropForeignKey(
                name: "FK_Subastas_StatusSubastas_CveStatusSubasta",
                table: "Subastas");

            migrationBuilder.DropForeignKey(
                name: "FK_Subastas_TiposSubasta_CveTipoSubasta",
                table: "Subastas");

            migrationBuilder.DropForeignKey(
                name: "FK_Subastas_Usuarios_CveUsuarioGanador",
                table: "Subastas");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_TiposUsuario_CveTipoUsuario",
                table: "Usuarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehiculos_Productos_CveProducto",
                table: "Vehiculos");

            migrationBuilder.DropIndex(
                name: "IX_Vehiculos_CveProducto",
                table: "Vehiculos");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_CveTipoUsuario",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Subastas_CveProducto",
                table: "Subastas");

            migrationBuilder.DropIndex(
                name: "IX_Subastas_CveStatusSubasta",
                table: "Subastas");

            migrationBuilder.DropIndex(
                name: "IX_Subastas_CveTipoSubasta",
                table: "Subastas");

            migrationBuilder.DropIndex(
                name: "IX_Subastas_CveUsuarioGanador",
                table: "Subastas");

            migrationBuilder.DropIndex(
                name: "IX_Productos_CveCategoria",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_Productos_CveCondicion",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_Productos_CveStatusProducto",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_Productos_CveUsuario",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_Pagos_CveStuatusPago",
                table: "Pagos");

            migrationBuilder.DropIndex(
                name: "IX_Pagos_CveSubasta",
                table: "Pagos");

            migrationBuilder.DropIndex(
                name: "IX_Ofertas_CveSubasta",
                table: "Ofertas");

            migrationBuilder.DropIndex(
                name: "IX_Ofertas_CveUsuario",
                table: "Ofertas");

            migrationBuilder.DropIndex(
                name: "IX_Notificaciones_CveOferta",
                table: "Notificaciones");

            migrationBuilder.DropIndex(
                name: "IX_Notificaciones_CveSubasta",
                table: "Notificaciones");

            migrationBuilder.DropIndex(
                name: "IX_Notificaciones_CveTipoNotificacion",
                table: "Notificaciones");

            migrationBuilder.DropIndex(
                name: "IX_Notificaciones_CveUsuario",
                table: "Notificaciones");

            migrationBuilder.DropIndex(
                name: "IX_Inmuebles_CveProducto",
                table: "Inmuebles");

            migrationBuilder.DropIndex(
                name: "IX_FotosProducto_CveProducto",
                table: "FotosProducto");

            migrationBuilder.DropIndex(
                name: "IX_Calificaciones_CveSubasta",
                table: "Calificaciones");

            migrationBuilder.AddColumn<int>(
                name: "ProductoRefIdProducto",
                table: "Vehiculos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoUsuarioRefIdTipoUsuario",
                table: "Usuarios",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductoRefIdProducto",
                table: "Subastas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusSubastaRefIdStatusSubasta",
                table: "Subastas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoSubastaRefIdTipoSubasta",
                table: "Subastas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioGanadorRefIdUsuario",
                table: "Subastas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoriaRefIdCategoria",
                table: "Productos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CondicionRefIdCondicion",
                table: "Productos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusProductoRefIdStatusProducto",
                table: "Productos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioRefIdUsuario",
                table: "Productos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusPagoRefIdStatusPago",
                table: "Pagos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubastaRefIdSubasta",
                table: "Pagos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubastaRefIdSubasta",
                table: "Ofertas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioRefIdUsuario",
                table: "Ofertas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OfertaRefIdOferta",
                table: "Notificaciones",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubastaRefIdSubasta",
                table: "Notificaciones",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoNotifiacionRefIdTipoNotificacion",
                table: "Notificaciones",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioRefIdUsuario",
                table: "Notificaciones",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductoRefIdProducto",
                table: "Inmuebles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductoRefIdProducto",
                table: "FotosProducto",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehiculos_ProductoRefIdProducto",
                table: "Vehiculos",
                column: "ProductoRefIdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_TipoUsuarioRefIdTipoUsuario",
                table: "Usuarios",
                column: "TipoUsuarioRefIdTipoUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Subastas_ProductoRefIdProducto",
                table: "Subastas",
                column: "ProductoRefIdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Subastas_StatusSubastaRefIdStatusSubasta",
                table: "Subastas",
                column: "StatusSubastaRefIdStatusSubasta");

            migrationBuilder.CreateIndex(
                name: "IX_Subastas_TipoSubastaRefIdTipoSubasta",
                table: "Subastas",
                column: "TipoSubastaRefIdTipoSubasta");

            migrationBuilder.CreateIndex(
                name: "IX_Subastas_UsuarioGanadorRefIdUsuario",
                table: "Subastas",
                column: "UsuarioGanadorRefIdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_CategoriaRefIdCategoria",
                table: "Productos",
                column: "CategoriaRefIdCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_CondicionRefIdCondicion",
                table: "Productos",
                column: "CondicionRefIdCondicion");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_StatusProductoRefIdStatusProducto",
                table: "Productos",
                column: "StatusProductoRefIdStatusProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_UsuarioRefIdUsuario",
                table: "Productos",
                column: "UsuarioRefIdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_StatusPagoRefIdStatusPago",
                table: "Pagos",
                column: "StatusPagoRefIdStatusPago");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_SubastaRefIdSubasta",
                table: "Pagos",
                column: "SubastaRefIdSubasta");

            migrationBuilder.CreateIndex(
                name: "IX_Ofertas_SubastaRefIdSubasta",
                table: "Ofertas",
                column: "SubastaRefIdSubasta");

            migrationBuilder.CreateIndex(
                name: "IX_Ofertas_UsuarioRefIdUsuario",
                table: "Ofertas",
                column: "UsuarioRefIdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_OfertaRefIdOferta",
                table: "Notificaciones",
                column: "OfertaRefIdOferta");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_SubastaRefIdSubasta",
                table: "Notificaciones",
                column: "SubastaRefIdSubasta");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_TipoNotifiacionRefIdTipoNotificacion",
                table: "Notificaciones",
                column: "TipoNotifiacionRefIdTipoNotificacion");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_UsuarioRefIdUsuario",
                table: "Notificaciones",
                column: "UsuarioRefIdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Inmuebles_ProductoRefIdProducto",
                table: "Inmuebles",
                column: "ProductoRefIdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_FotosProducto_ProductoRefIdProducto",
                table: "FotosProducto",
                column: "ProductoRefIdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Calificaciones_CveSubasta",
                table: "Calificaciones",
                column: "CveSubasta");

            migrationBuilder.AddForeignKey(
                name: "FK_FotosProducto_Productos_ProductoRefIdProducto",
                table: "FotosProducto",
                column: "ProductoRefIdProducto",
                principalTable: "Productos",
                principalColumn: "IdProducto");

            migrationBuilder.AddForeignKey(
                name: "FK_Inmuebles_Productos_ProductoRefIdProducto",
                table: "Inmuebles",
                column: "ProductoRefIdProducto",
                principalTable: "Productos",
                principalColumn: "IdProducto");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificaciones_Ofertas_OfertaRefIdOferta",
                table: "Notificaciones",
                column: "OfertaRefIdOferta",
                principalTable: "Ofertas",
                principalColumn: "IdOferta");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificaciones_Subastas_SubastaRefIdSubasta",
                table: "Notificaciones",
                column: "SubastaRefIdSubasta",
                principalTable: "Subastas",
                principalColumn: "IdSubasta");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificaciones_TiposNotificacion_TipoNotifiacionRefIdTipoNotificacion",
                table: "Notificaciones",
                column: "TipoNotifiacionRefIdTipoNotificacion",
                principalTable: "TiposNotificacion",
                principalColumn: "IdTipoNotificacion");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificaciones_Usuarios_UsuarioRefIdUsuario",
                table: "Notificaciones",
                column: "UsuarioRefIdUsuario",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Ofertas_Subastas_SubastaRefIdSubasta",
                table: "Ofertas",
                column: "SubastaRefIdSubasta",
                principalTable: "Subastas",
                principalColumn: "IdSubasta");

            migrationBuilder.AddForeignKey(
                name: "FK_Ofertas_Usuarios_UsuarioRefIdUsuario",
                table: "Ofertas",
                column: "UsuarioRefIdUsuario",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Pagos_StatusPagos_StatusPagoRefIdStatusPago",
                table: "Pagos",
                column: "StatusPagoRefIdStatusPago",
                principalTable: "StatusPagos",
                principalColumn: "IdStatusPago");

            migrationBuilder.AddForeignKey(
                name: "FK_Pagos_Subastas_SubastaRefIdSubasta",
                table: "Pagos",
                column: "SubastaRefIdSubasta",
                principalTable: "Subastas",
                principalColumn: "IdSubasta");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Categorias_CategoriaRefIdCategoria",
                table: "Productos",
                column: "CategoriaRefIdCategoria",
                principalTable: "Categorias",
                principalColumn: "IdCategoria");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Condiciones_CondicionRefIdCondicion",
                table: "Productos",
                column: "CondicionRefIdCondicion",
                principalTable: "Condiciones",
                principalColumn: "IdCondicion");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_StatusProductos_StatusProductoRefIdStatusProducto",
                table: "Productos",
                column: "StatusProductoRefIdStatusProducto",
                principalTable: "StatusProductos",
                principalColumn: "IdStatusProducto");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Usuarios_UsuarioRefIdUsuario",
                table: "Productos",
                column: "UsuarioRefIdUsuario",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Subastas_Productos_ProductoRefIdProducto",
                table: "Subastas",
                column: "ProductoRefIdProducto",
                principalTable: "Productos",
                principalColumn: "IdProducto");

            migrationBuilder.AddForeignKey(
                name: "FK_Subastas_StatusSubastas_StatusSubastaRefIdStatusSubasta",
                table: "Subastas",
                column: "StatusSubastaRefIdStatusSubasta",
                principalTable: "StatusSubastas",
                principalColumn: "IdStatusSubasta");

            migrationBuilder.AddForeignKey(
                name: "FK_Subastas_TiposSubasta_TipoSubastaRefIdTipoSubasta",
                table: "Subastas",
                column: "TipoSubastaRefIdTipoSubasta",
                principalTable: "TiposSubasta",
                principalColumn: "IdTipoSubasta");

            migrationBuilder.AddForeignKey(
                name: "FK_Subastas_Usuarios_UsuarioGanadorRefIdUsuario",
                table: "Subastas",
                column: "UsuarioGanadorRefIdUsuario",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_TiposUsuario_TipoUsuarioRefIdTipoUsuario",
                table: "Usuarios",
                column: "TipoUsuarioRefIdTipoUsuario",
                principalTable: "TiposUsuario",
                principalColumn: "IdTipoUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehiculos_Productos_ProductoRefIdProducto",
                table: "Vehiculos",
                column: "ProductoRefIdProducto",
                principalTable: "Productos",
                principalColumn: "IdProducto");
        }
    }
}
