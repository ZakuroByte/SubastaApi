using System.ComponentModel.DataAnnotations;

namespace SubastaApi.Entidades
{
    public class Subasta
    {
        public int IdSUbasta { get; set; }
        [Required]
        public required float PrecioInicial { get; set; }
        [Required]
        public required float PrecioActual { get; set; }
        [Required]
        public required DateTime FechaInicio { get; set; }
        [Required]
        public required DateTime FechaFinal { get; set; }
        [Required]
        public required int Cantidad { get; set; }
        [Required]
        public required int CveTipoSubasta { get; set; }
        [Required]
        public required int CveProducto { get; set; }
        [Required]
        public required int CveStatus { get; set; }
        public int? CveUsuarioGanador { get; set; }
    }
}