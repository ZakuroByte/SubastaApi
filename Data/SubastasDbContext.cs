using Microsoft.EntityFrameworkCore;
using SubastaApi.Entidades;

namespace SubastaApi.Data
{
    public class SubastaDbContext : DbContext
    {
        public SubastaDbContext(DbContextOptions<SubastaDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<TipoUsuario> TiposUsuario { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Condicion> Condiciones { get; set; }
        public DbSet<StatusProducto> StatusProductos { get; set; }
        public DbSet<FotoProducto> FotosProducto { get; set; }
        public DbSet<Vehiculo> Vehiculos { get; set; }
        public DbSet<Inmueble> Inmuebles { get; set; }
        public DbSet<Subasta> Subastas { get; set; }
        public DbSet<TipoSubasta> TiposSubasta { get; set; }
        public DbSet<StatusSubasta> StatusSubastas { get; set; }
        public DbSet<Oferta> Ofertas { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<StatusPago> StatusPagos { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }
        public DbSet<TipoNotificacion> TiposNotificacion { get; set; }
        public DbSet<Calificacion> Calificaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Calificacion

            modelBuilder.Entity<Calificacion>()
            .HasKey(c => c.IdCalificacion);

            modelBuilder.Entity<Calificacion>()
                .HasOne(c => c.UsuarioCalificado)
                .WithMany(u => u.CalificacionesRecibidas)
                .HasForeignKey(c => c.CveUsuarioCalificado)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Calificacion>()
                .HasOne(c => c.UsuarioCalificador)
                .WithMany(u => u.CalificacionesDadas)
                .HasForeignKey(c => c.CveUsuarioCalificador)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Calificacion>()
                .HasOne(c => c.SubastaRef)
                .WithOne(s => s.CalificacionRef)
                .HasForeignKey<Calificacion>(c => c.CveSubasta)
                .OnDelete(DeleteBehavior.Restrict);    

            modelBuilder.Entity<Categoria>()
            .HasKey(c => c.IdCategoria);

            modelBuilder.Entity<Condicion>()
            .HasKey(c => c.IdCondicion);

            //FotoProducto
            modelBuilder.Entity<FotoProducto>()
            .HasKey(c => c.IdFoto);

            modelBuilder.Entity<FotoProducto>()
                .HasOne(c => c.ProductoRef)
                .WithMany(s => s.Fotos)
                .HasForeignKey(c => c.CveProducto)
                .OnDelete(DeleteBehavior.Restrict);

            //Inmueble
            modelBuilder.Entity<Inmueble>()
            .HasKey(c => c.IdInmueble);

            modelBuilder.Entity<Inmueble>()
                .HasOne(c => c.ProductoRef)
                .WithOne(s => s.InmuebleRef)
                .HasForeignKey<Inmueble>(c => c.CveProducto)
                .OnDelete(DeleteBehavior.Restrict);

            //Notificaion
            modelBuilder.Entity<Notificacion>()
            .HasKey(c => c.IdNotificacion);

            modelBuilder.Entity<Notificacion>()
                .HasOne(c => c.UsuarioRef)
                .WithMany(s => s.Notificaciones)
                .HasForeignKey(c => c.CveUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notificacion>()
                .HasOne(c => c.TipoNotificacionRef)
                .WithMany(s => s.Notificaciones)
                .HasForeignKey(c => c.CveTipoNotificacion)
                .OnDelete(DeleteBehavior.Restrict);
           
            modelBuilder.Entity<Notificacion>()
                .HasOne(c => c.OfertaRef)
                .WithMany(s => s.Notificaciones)
                .HasForeignKey(c => c.CveOferta)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Notificacion>()
                .HasOne(c => c.SubastaRef)
                .WithMany(s => s.Notificaciones)
                .HasForeignKey(c => c.CveSubasta)
                .OnDelete(DeleteBehavior.Restrict);
            
            //Oferta
            modelBuilder.Entity<Oferta>()
            .HasKey(c => c.IdOferta);

            modelBuilder.Entity<Oferta>()
                .HasOne(c => c.UsuarioRef)
                .WithMany(s => s.Ofertas)
                .HasForeignKey(c => c.CveUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Oferta>()
                .HasOne(c => c.SubastaRef)
                .WithMany(s => s.Ofertas)
                .HasForeignKey(c => c.CveSubasta)
                .OnDelete(DeleteBehavior.Restrict);

            //Pago
            modelBuilder.Entity<Pago>()
            .HasKey(c => c.IdPago);

            modelBuilder.Entity<Pago>()
                .HasOne(c => c.SubastaRef)
                .WithOne(s => s.PagoRef)
                .HasForeignKey<Pago>(c => c.CveSubasta)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Pago>()
                .HasOne(c => c.StatusPagoRef)
                .WithMany(s => s.Pagos)
                .HasForeignKey(c => c.CveStatusPago)
                .OnDelete(DeleteBehavior.Restrict);

            //Producto
            modelBuilder.Entity<Producto>()
            .HasKey(c => c.IdProducto);

            modelBuilder.Entity<Producto>()
                .HasOne(c => c.CategoriaRef)
                .WithMany(s => s.Productos)
                .HasForeignKey(c => c.CveCategoria)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Producto>()
                .HasOne(c => c.CondicionRef)
                .WithMany(s => s.Productos)
                .HasForeignKey(c => c.CveCondicion)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Producto>()
                .HasOne(c => c.UsuarioRef)
                .WithMany(s => s.Productos)
                .HasForeignKey(c => c.CveUsuario)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Producto>()
                .HasOne(c => c.StatusProductoRef)
                .WithMany(s => s.Productos)
                .HasForeignKey(c => c.CveStatusProducto)
                .OnDelete(DeleteBehavior.Restrict);

            //StatusPago
            modelBuilder.Entity<StatusPago>()
            .HasKey(c => c.IdStatusPago);

            //StatusProducto
            modelBuilder.Entity<StatusProducto>()
            .HasKey(c => c.IdStatusProducto);

            //StatusSubasta
            modelBuilder.Entity<StatusSubasta>()
            .HasKey(c => c.IdStatusSubasta);

            //Subasta
            modelBuilder.Entity<Subasta>()
            .HasKey(c => c.IdSubasta);

            modelBuilder.Entity<Subasta>()
                .HasOne(c => c.TipoSubastaRef)
                .WithMany(s => s.Subastas)
                .HasForeignKey(c => c.CveTipoSubasta)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Subasta>()
                .HasOne(c => c.ProductoRef)
                .WithMany(s => s.Subastas)
                .HasForeignKey(c => c.CveProducto)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Subasta>()
                .HasOne(c => c.StatusSubastaRef)
                .WithMany(s => s.Subastas)
                .HasForeignKey(c => c.CveStatusSubasta)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Subasta>()
                .HasOne(c => c.UsuarioGanadorRef)
                .WithMany(s => s.Subastas)
                .HasForeignKey(c => c.CveUsuarioGanador)
                .OnDelete(DeleteBehavior.Restrict);

            //TipoNotificacion
            modelBuilder.Entity<TipoNotificacion>()
            .HasKey(c => c.IdTipoNotificacion);

            //TipoSubasta
            modelBuilder.Entity<TipoSubasta>()
            .HasKey(c => c.IdTipoSubasta);

            //TipoUsuario
            modelBuilder.Entity<TipoUsuario>()
            .HasKey(c => c.IdTipoUsuario);

            //Usuario
            modelBuilder.Entity<Usuario>()
            .HasKey(c => c.IdUsuario);

            modelBuilder.Entity<Usuario>()
                .HasOne(c => c.TipoUsuarioRef)
                .WithMany(s => s.Usuarios)
                .HasForeignKey(c => c.CveTipoUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            //Vehiculo
            modelBuilder.Entity<Vehiculo>()
            .HasKey(c => c.IdVehiculo);

            modelBuilder.Entity<Vehiculo>()
                .HasOne(c => c.ProductoRef)
                .WithOne(s => s.VehiculoRef)
                .HasForeignKey<Vehiculo>(c => c.CveProducto)
                .OnDelete(DeleteBehavior.Restrict);

            // Oferta
            modelBuilder.Entity<Oferta>()
                .Property(o => o.Monto)
                .HasPrecision(18, 2);

            // Pago
            modelBuilder.Entity<Pago>()
                .Property(p => p.Monto)
                .HasPrecision(18, 2);

            // Subasta
            modelBuilder.Entity<Subasta>()
                .Property(s => s.PrecioInicial)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Subasta>()
                .Property(s => s.PrecioActual)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Subasta>()
                .Property(s => s.Incremento)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Subasta>()
                .Property(s => s.Decremento)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TipoUsuario>().HasData(
                new TipoUsuario { IdTipoUsuario = 1, Descripcion = "Administrador" },
                new TipoUsuario { IdTipoUsuario = 2, Descripcion = "Vendedor" },
                new TipoUsuario { IdTipoUsuario = 3, Descripcion = "Comprador" }
            );

            modelBuilder.Entity<TipoSubasta>().HasData(
                new TipoSubasta { IdTipoSubasta = 1, Descripcion = "Inglesa" },
                new TipoSubasta { IdTipoSubasta = 2, Descripcion = "Holandesa" },
                new TipoSubasta { IdTipoSubasta = 3, Descripcion = "Sellada" }
            );

            modelBuilder.Entity<StatusSubasta>().HasData(
                new StatusSubasta { IdStatusSubasta = 1, Descripcion = "Pendiente" },
                new StatusSubasta { IdStatusSubasta = 2, Descripcion = "Activa" },
                new StatusSubasta { IdStatusSubasta = 3, Descripcion = "Finalizada" },
                new StatusSubasta { IdStatusSubasta = 4, Descripcion = "Cancelada" }
            );

            modelBuilder.Entity<StatusProducto>().HasData(
                new StatusProducto { IdStatusProducto = 1, Descripcion = "Disponible" },
                new StatusProducto { IdStatusProducto = 2, Descripcion = "En Subasta" },
                new StatusProducto { IdStatusProducto = 3, Descripcion = "Vendido" },
                new StatusProducto { IdStatusProducto = 4, Descripcion = "Retirado" }
            );

            modelBuilder.Entity<StatusPago>().HasData(
                new StatusPago { IdStatusPago = 1, Descripcion = "Pendiente" },
                new StatusPago { IdStatusPago = 2, Descripcion = "Pagado" },
                new StatusPago { IdStatusPago = 3, Descripcion = "Vencido" }
            );

            modelBuilder.Entity<TipoNotificacion>().HasData(
                new TipoNotificacion { IdTipoNotificacion = 1, Descripcion = "Oferta Recibida" },
                new TipoNotificacion { IdTipoNotificacion = 2, Descripcion = "Oferta Superada" },
                new TipoNotificacion { IdTipoNotificacion = 3, Descripcion = "Pago Pendiente" }
            );

            modelBuilder.Entity<Condicion>().HasData(
                new Condicion { IdCondicion = 1, Descripcion = "Nuevo" },
                new Condicion { IdCondicion = 2, Descripcion = "Usado" },
                new Condicion { IdCondicion = 3, Descripcion = "Reacondicionado" }
            );

            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { IdCategoria = 1, Descripcion = "Vehículo" },
                new Categoria { IdCategoria = 2, Descripcion = "Inmueble" },
                new Categoria { IdCategoria = 3, Descripcion = "Electronicos" },
                new Categoria { IdCategoria = 4, Descripcion = "Arte y coleccionables" },
                new Categoria { IdCategoria = 5, Descripcion = "Antiguedades" },
                new Categoria { IdCategoria = 6, Descripcion = "Ropas y accesorios" },
                new Categoria { IdCategoria = 7, Descripcion = "Articulos deportivos" },
                new Categoria { IdCategoria = 8, Descripcion = "Libros" },
                new Categoria { IdCategoria = 9, Descripcion = "Juguetes" },
                new Categoria { IdCategoria = 10, Descripcion = "Contenidos digitales" },
                new Categoria { IdCategoria = 11, Descripcion = "Entrada a eventos" }
            );
        }
    }
}