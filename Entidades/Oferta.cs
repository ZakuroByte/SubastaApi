using System.ComponentModel.DataAnnotations;

namespace SubastaApi.Entidades
{
    public class Oferta
    {
        public int IdOferta { get; set; }
        [Required]
        public required DateTime Fecha { get; set; }
        [Required]
        public required float Monto { get; set; }
        [Required]
        public required int CveUsuario { get; set; }
        [Required]
        public required int CveSubasta { get; set; }
    }
}