using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SubastaApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    IdCategoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.IdCategoria);
                });

            migrationBuilder.CreateTable(
                name: "Condiciones",
                columns: table => new
                {
                    IdCondicion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Condiciones", x => x.IdCondicion);
                });

            migrationBuilder.CreateTable(
                name: "StatusPagos",
                columns: table => new
                {
                    IdStatusPago = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusPagos", x => x.IdStatusPago);
                });

            migrationBuilder.CreateTable(
                name: "StatusProductos",
                columns: table => new
                {
                    IdStatusProducto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusProductos", x => x.IdStatusProducto);
                });

            migrationBuilder.CreateTable(
                name: "StatusSubastas",
                columns: table => new
                {
                    IdStatusSubasta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusSubastas", x => x.IdStatusSubasta);
                });

            migrationBuilder.CreateTable(
                name: "TiposNotificacion",
                columns: table => new
                {
                    IdTipoNotificacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposNotificacion", x => x.IdTipoNotificacion);
                });

            migrationBuilder.CreateTable(
                name: "TiposSubasta",
                columns: table => new
                {
                    IdTipoSubasta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposSubasta", x => x.IdTipoSubasta);
                });

            migrationBuilder.CreateTable(
                name: "TiposUsuario",
                columns: table => new
                {
                    IdTipoUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposUsuario", x => x.IdTipoUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contrasenia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApellidoPaterno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApellidoMaterno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Calificacion = table.Column<int>(type: "int", nullable: true),
                    CveTipoUsuario = table.Column<int>(type: "int", nullable: false),
                    TipoUsuarioRefIdTipoUsuario = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_Usuarios_TiposUsuario_TipoUsuarioRefIdTipoUsuario",
                        column: x => x.TipoUsuarioRefIdTipoUsuario,
                        principalTable: "TiposUsuario",
                        principalColumn: "IdTipoUsuario");
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    IdProducto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CveCategoria = table.Column<int>(type: "int", nullable: false),
                    CveCondicion = table.Column<int>(type: "int", nullable: false),
                    CveUsuario = table.Column<int>(type: "int", nullable: false),
                    CveStatusProducto = table.Column<int>(type: "int", nullable: false),
                    CategoriaRefIdCategoria = table.Column<int>(type: "int", nullable: true),
                    CondicionRefIdCondicion = table.Column<int>(type: "int", nullable: true),
                    UsuarioRefIdUsuario = table.Column<int>(type: "int", nullable: true),
                    StatusProductoRefIdStatusProducto = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.IdProducto);
                    table.ForeignKey(
                        name: "FK_Productos_Categorias_CategoriaRefIdCategoria",
                        column: x => x.CategoriaRefIdCategoria,
                        principalTable: "Categorias",
                        principalColumn: "IdCategoria");
                    table.ForeignKey(
                        name: "FK_Productos_Condiciones_CondicionRefIdCondicion",
                        column: x => x.CondicionRefIdCondicion,
                        principalTable: "Condiciones",
                        principalColumn: "IdCondicion");
                    table.ForeignKey(
                        name: "FK_Productos_StatusProductos_StatusProductoRefIdStatusProducto",
                        column: x => x.StatusProductoRefIdStatusProducto,
                        principalTable: "StatusProductos",
                        principalColumn: "IdStatusProducto");
                    table.ForeignKey(
                        name: "FK_Productos_Usuarios_UsuarioRefIdUsuario",
                        column: x => x.UsuarioRefIdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario");
                });

            migrationBuilder.CreateTable(
                name: "FotosProducto",
                columns: table => new
                {
                    IdFoto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CveProducto = table.Column<int>(type: "int", nullable: false),
                    ProductoRefIdProducto = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FotosProducto", x => x.IdFoto);
                    table.ForeignKey(
                        name: "FK_FotosProducto_Productos_ProductoRefIdProducto",
                        column: x => x.ProductoRefIdProducto,
                        principalTable: "Productos",
                        principalColumn: "IdProducto");
                });

            migrationBuilder.CreateTable(
                name: "Inmuebles",
                columns: table => new
                {
                    IdInmueble = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SuperficieTerreno = table.Column<int>(type: "int", nullable: false),
                    SuperficieConstruida = table.Column<int>(type: "int", nullable: false),
                    NumeroHabitaciones = table.Column<int>(type: "int", nullable: false),
                    UrlDocumentacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CveProducto = table.Column<int>(type: "int", nullable: false),
                    ProductoRefIdProducto = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inmuebles", x => x.IdInmueble);
                    table.ForeignKey(
                        name: "FK_Inmuebles_Productos_ProductoRefIdProducto",
                        column: x => x.ProductoRefIdProducto,
                        principalTable: "Productos",
                        principalColumn: "IdProducto");
                });

            migrationBuilder.CreateTable(
                name: "Subastas",
                columns: table => new
                {
                    IdSubasta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrecioInicial = table.Column<float>(type: "real", nullable: false),
                    PrecioActual = table.Column<float>(type: "real", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFinal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    CveTipoSubasta = table.Column<int>(type: "int", nullable: false),
                    CveProducto = table.Column<int>(type: "int", nullable: false),
                    CveStatusSubasta = table.Column<int>(type: "int", nullable: false),
                    CveUsuarioGanador = table.Column<int>(type: "int", nullable: true),
                    TipoSubastaRefIdTipoSubasta = table.Column<int>(type: "int", nullable: true),
                    ProductoRefIdProducto = table.Column<int>(type: "int", nullable: true),
                    StatusSubastaRefIdStatusSubasta = table.Column<int>(type: "int", nullable: true),
                    UsuarioGanadorRefIdUsuario = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subastas", x => x.IdSubasta);
                    table.ForeignKey(
                        name: "FK_Subastas_Productos_ProductoRefIdProducto",
                        column: x => x.ProductoRefIdProducto,
                        principalTable: "Productos",
                        principalColumn: "IdProducto");
                    table.ForeignKey(
                        name: "FK_Subastas_StatusSubastas_StatusSubastaRefIdStatusSubasta",
                        column: x => x.StatusSubastaRefIdStatusSubasta,
                        principalTable: "StatusSubastas",
                        principalColumn: "IdStatusSubasta");
                    table.ForeignKey(
                        name: "FK_Subastas_TiposSubasta_TipoSubastaRefIdTipoSubasta",
                        column: x => x.TipoSubastaRefIdTipoSubasta,
                        principalTable: "TiposSubasta",
                        principalColumn: "IdTipoSubasta");
                    table.ForeignKey(
                        name: "FK_Subastas_Usuarios_UsuarioGanadorRefIdUsuario",
                        column: x => x.UsuarioGanadorRefIdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario");
                });

            migrationBuilder.CreateTable(
                name: "Vehiculos",
                columns: table => new
                {
                    IdVehiculo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Marca = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Modelo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Anio = table.Column<int>(type: "int", nullable: false),
                    Kilometraje = table.Column<float>(type: "real", nullable: false),
                    NumeroSerie = table.Column<int>(type: "int", nullable: false),
                    UrlDocumentacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CveProducto = table.Column<int>(type: "int", nullable: false),
                    ProductoRefIdProducto = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehiculos", x => x.IdVehiculo);
                    table.ForeignKey(
                        name: "FK_Vehiculos_Productos_ProductoRefIdProducto",
                        column: x => x.ProductoRefIdProducto,
                        principalTable: "Productos",
                        principalColumn: "IdProducto");
                });

            migrationBuilder.CreateTable(
                name: "Calificaciones",
                columns: table => new
                {
                    IdCalificacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Estrellas = table.Column<int>(type: "int", nullable: false),
                    Comentario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CveUsuarioCalificado = table.Column<int>(type: "int", nullable: false),
                    CveUsuarioCalificador = table.Column<int>(type: "int", nullable: false),
                    CveSubasta = table.Column<int>(type: "int", nullable: false),
                    SubastaRefIdSubasta = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calificaciones", x => x.IdCalificacion);
                    table.ForeignKey(
                        name: "FK_Calificaciones_Subastas_SubastaRefIdSubasta",
                        column: x => x.SubastaRefIdSubasta,
                        principalTable: "Subastas",
                        principalColumn: "IdSubasta");
                    table.ForeignKey(
                        name: "FK_Calificaciones_Usuarios_CveUsuarioCalificado",
                        column: x => x.CveUsuarioCalificado,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Calificaciones_Usuarios_CveUsuarioCalificador",
                        column: x => x.CveUsuarioCalificador,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ofertas",
                columns: table => new
                {
                    IdOferta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Monto = table.Column<float>(type: "real", nullable: false),
                    CveUsuario = table.Column<int>(type: "int", nullable: false),
                    CveSubasta = table.Column<int>(type: "int", nullable: false),
                    UsuarioRefIdUsuario = table.Column<int>(type: "int", nullable: true),
                    SubastaRefIdSubasta = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ofertas", x => x.IdOferta);
                    table.ForeignKey(
                        name: "FK_Ofertas_Subastas_SubastaRefIdSubasta",
                        column: x => x.SubastaRefIdSubasta,
                        principalTable: "Subastas",
                        principalColumn: "IdSubasta");
                    table.ForeignKey(
                        name: "FK_Ofertas_Usuarios_UsuarioRefIdUsuario",
                        column: x => x.UsuarioRefIdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario");
                });

            migrationBuilder.CreateTable(
                name: "Pagos",
                columns: table => new
                {
                    IdPago = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Monto = table.Column<float>(type: "real", nullable: false),
                    FechaLimite = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaRealizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CveSubasta = table.Column<int>(type: "int", nullable: false),
                    CveStuatusPago = table.Column<int>(type: "int", nullable: false),
                    SubastaRefIdSubasta = table.Column<int>(type: "int", nullable: true),
                    StatusPagoRefIdStatusPago = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagos", x => x.IdPago);
                    table.ForeignKey(
                        name: "FK_Pagos_StatusPagos_StatusPagoRefIdStatusPago",
                        column: x => x.StatusPagoRefIdStatusPago,
                        principalTable: "StatusPagos",
                        principalColumn: "IdStatusPago");
                    table.ForeignKey(
                        name: "FK_Pagos_Subastas_SubastaRefIdSubasta",
                        column: x => x.SubastaRefIdSubasta,
                        principalTable: "Subastas",
                        principalColumn: "IdSubasta");
                });

            migrationBuilder.CreateTable(
                name: "Notificaciones",
                columns: table => new
                {
                    IdNotificacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaEnvio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Leida = table.Column<bool>(type: "bit", nullable: false),
                    CveUsuario = table.Column<int>(type: "int", nullable: false),
                    CveTipoNotificacion = table.Column<int>(type: "int", nullable: false),
                    CveOferta = table.Column<int>(type: "int", nullable: true),
                    CveSubasta = table.Column<int>(type: "int", nullable: true),
                    UsuarioRefIdUsuario = table.Column<int>(type: "int", nullable: true),
                    TipoNotifiacionRefIdTipoNotificacion = table.Column<int>(type: "int", nullable: true),
                    OfertaRefIdOferta = table.Column<int>(type: "int", nullable: true),
                    SubastaRefIdSubasta = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificaciones", x => x.IdNotificacion);
                    table.ForeignKey(
                        name: "FK_Notificaciones_Ofertas_OfertaRefIdOferta",
                        column: x => x.OfertaRefIdOferta,
                        principalTable: "Ofertas",
                        principalColumn: "IdOferta");
                    table.ForeignKey(
                        name: "FK_Notificaciones_Subastas_SubastaRefIdSubasta",
                        column: x => x.SubastaRefIdSubasta,
                        principalTable: "Subastas",
                        principalColumn: "IdSubasta");
                    table.ForeignKey(
                        name: "FK_Notificaciones_TiposNotificacion_TipoNotifiacionRefIdTipoNotificacion",
                        column: x => x.TipoNotifiacionRefIdTipoNotificacion,
                        principalTable: "TiposNotificacion",
                        principalColumn: "IdTipoNotificacion");
                    table.ForeignKey(
                        name: "FK_Notificaciones_Usuarios_UsuarioRefIdUsuario",
                        column: x => x.UsuarioRefIdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario");
                });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "IdCategoria", "Descripcion" },
                values: new object[,]
                {
                    { 1, "Vehículo" },
                    { 2, "Inmueble" },
                    { 3, "Electronicos" },
                    { 4, "Arte y coleccionables" },
                    { 5, "Antiguedades" },
                    { 6, "Ropas y accesorios" },
                    { 7, "Articulos deportivos" },
                    { 8, "Libros" },
                    { 9, "Juguetes" },
                    { 10, "Contenidos digitales" },
                    { 11, "Entrada a eventos" }
                });

            migrationBuilder.InsertData(
                table: "Condiciones",
                columns: new[] { "IdCondicion", "Descripcion" },
                values: new object[,]
                {
                    { 1, "Nuevo" },
                    { 2, "Usado" },
                    { 3, "Reacondicionado" }
                });

            migrationBuilder.InsertData(
                table: "StatusPagos",
                columns: new[] { "IdStatusPago", "Descripcion" },
                values: new object[,]
                {
                    { 1, "Pendiente" },
                    { 2, "Pagado" },
                    { 3, "Vencido" }
                });

            migrationBuilder.InsertData(
                table: "StatusProductos",
                columns: new[] { "IdStatusProducto", "Descripcion" },
                values: new object[,]
                {
                    { 1, "Disponible" },
                    { 2, "En Subasta" },
                    { 3, "Vendido" },
                    { 4, "Retirado" }
                });

            migrationBuilder.InsertData(
                table: "StatusSubastas",
                columns: new[] { "IdStatusSubasta", "Descripcion" },
                values: new object[,]
                {
                    { 1, "Pendiente" },
                    { 2, "Activa" },
                    { 3, "Finalizada" },
                    { 4, "Cancelada" }
                });

            migrationBuilder.InsertData(
                table: "TiposNotificacion",
                columns: new[] { "IdTipoNotificacion", "Descripcion" },
                values: new object[,]
                {
                    { 1, "Oferta Recibida" },
                    { 2, "Oferta Superada" },
                    { 3, "Pago Pendiente" }
                });

            migrationBuilder.InsertData(
                table: "TiposSubasta",
                columns: new[] { "IdTipoSubasta", "Descripcion" },
                values: new object[,]
                {
                    { 1, "Inglesa" },
                    { 2, "Holandesa" },
                    { 3, "Sellada" }
                });

            migrationBuilder.InsertData(
                table: "TiposUsuario",
                columns: new[] { "IdTipoUsuario", "Descripcion" },
                values: new object[,]
                {
                    { 1, "Administrador" },
                    { 2, "Vendedor" },
                    { 3, "Comprador" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Calificaciones_CveUsuarioCalificado",
                table: "Calificaciones",
                column: "CveUsuarioCalificado");

            migrationBuilder.CreateIndex(
                name: "IX_Calificaciones_CveUsuarioCalificador",
                table: "Calificaciones",
                column: "CveUsuarioCalificador");

            migrationBuilder.CreateIndex(
                name: "IX_Calificaciones_SubastaRefIdSubasta",
                table: "Calificaciones",
                column: "SubastaRefIdSubasta");

            migrationBuilder.CreateIndex(
                name: "IX_FotosProducto_ProductoRefIdProducto",
                table: "FotosProducto",
                column: "ProductoRefIdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Inmuebles_ProductoRefIdProducto",
                table: "Inmuebles",
                column: "ProductoRefIdProducto");

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
                name: "IX_Ofertas_SubastaRefIdSubasta",
                table: "Ofertas",
                column: "SubastaRefIdSubasta");

            migrationBuilder.CreateIndex(
                name: "IX_Ofertas_UsuarioRefIdUsuario",
                table: "Ofertas",
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
                name: "IX_Usuarios_TipoUsuarioRefIdTipoUsuario",
                table: "Usuarios",
                column: "TipoUsuarioRefIdTipoUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Vehiculos_ProductoRefIdProducto",
                table: "Vehiculos",
                column: "ProductoRefIdProducto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Calificaciones");

            migrationBuilder.DropTable(
                name: "FotosProducto");

            migrationBuilder.DropTable(
                name: "Inmuebles");

            migrationBuilder.DropTable(
                name: "Notificaciones");

            migrationBuilder.DropTable(
                name: "Pagos");

            migrationBuilder.DropTable(
                name: "Vehiculos");

            migrationBuilder.DropTable(
                name: "Ofertas");

            migrationBuilder.DropTable(
                name: "TiposNotificacion");

            migrationBuilder.DropTable(
                name: "StatusPagos");

            migrationBuilder.DropTable(
                name: "Subastas");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "StatusSubastas");

            migrationBuilder.DropTable(
                name: "TiposSubasta");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Condiciones");

            migrationBuilder.DropTable(
                name: "StatusProductos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "TiposUsuario");
        }
    }
}
