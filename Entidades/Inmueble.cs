using System.ComponentModel.DataAnnotations;

namespace SubastaApi.Entidades
{
    public class Inmueble
    {
        public int IdInmueble { get; set; }
        [Required]
        public required int SuperficieTerreno { get; set; }
        [Required]
        public required int SuperficieConstruida { get; set; }
        [Required]
        public required int NumeroHabitaciones { get; set; }
        [Required]
        public required string UrlDocumentacion { get; set; }
        [Required]
        public required int CveProducto { get; set; }
    }
}