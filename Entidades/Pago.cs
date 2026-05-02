using System.ComponentModel.DataAnnotations;

namespace SubastaApi.Entidades
{
    public class Pago
    {
        public int IdPago { get; set; }
        [Required]
        public required float Monto { get; set; }
        [Required]
        public required DateTime FechaLimite { get; set; }
        public DateTime? FechaRealizacion { get; set; }
        [Required]
        public required int CveSubasta { get; set; }
        [Required]
        public required int CveStuatus { get; set; }
    }
}