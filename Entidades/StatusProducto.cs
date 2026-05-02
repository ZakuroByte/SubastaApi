using System.ComponentModel.DataAnnotations;

namespace SubastaApi.Entidades
{
    public class StatusProducto
    {
        public int IdStatus { get; set; }
        [Required]
        public required string Descripcion { get; set; }
    }
}