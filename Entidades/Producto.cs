using System.ComponentModel.DataAnnotations;

namespace SubastaApi.Entidades
{
    public class Producto
    {
        public int IdProducto { get; set; }
        [Required]
        public required string Nombre { get; set; }
        [Required]
        public required string Descripcion { get; set; }
        [Required]
        public required string Ubicacion { get; set; }
        public int CveCategoria { get; set; }
        public int CveCondicion { get; set; }
        public int CveUsuario { get; set; }
        public int CveStatusProducto { get; set; }

        public Categoria? CategoriaRef { get; set; }
        public Condicion? CondicionRef { get; set; }
        public Usuario? UsuarioRef { get; set; }
        public Vehiculo? VehiculoRef { get; set; }
        public Inmueble? InmuebleRef { get; set; }
        public StatusProducto? StatusProductoRef { get; set; }
        public ICollection<FotoProducto> Fotos { get; set; } = [];
        public ICollection<Subasta> Subastas { get; set; } = [];
        
    }
}