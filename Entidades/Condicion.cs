using System.ComponentModel.DataAnnotations;

namespace SubastaApi.Entidades
{
    public class Condicion
    {
        public int IdCondicion { get; set; }
        [Required]
        public required string Descripcion { get; set; }
        
        public ICollection<Producto> Productos { get; set; } = [];
    }
}