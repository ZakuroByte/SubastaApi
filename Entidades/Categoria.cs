using System.ComponentModel.DataAnnotations;

namespace SubastaApi.Entidades
{
    public class Categoria
    {
        public int IdCategoria { get; set; }
        [Required]
        public required string Descripcion { get; set; }

        public ICollection<Producto> Productos { get; set; } = [];
    }
}