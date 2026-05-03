using System.ComponentModel.DataAnnotations;

namespace SubastaApi.Entidades
{
    public class StatusProducto
    {
        public int IdStatusProducto { get; set; }
        [Required]
        public required string Descripcion { get; set; }

        public ICollection<Producto> Productos { get; set; } = [];
    }
}