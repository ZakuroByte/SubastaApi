using System.ComponentModel.DataAnnotations;

namespace SubastaApi.Entidades
{
    public class StatusPago
    {
        public int IdStatusPago { get; set; }
        [Required]
        public required string Descripcion { get; set; }
    }
}