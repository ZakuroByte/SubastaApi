using System.ComponentModel.DataAnnotations;

namespace SubastaApi.Entidades
{
    public class Vehiculo
    {
        public int IdVehiculo { get; set; }
        [Required]
        public required string Marca { get; set; }
        [Required]
        public required string Modelo { get; set; }
        [Required]
        public required int Anio { get; set; }
        [Required]
        public required float Kilometraje { get; set; }
        [Required]
        public required int NumeroSerie { get; set; }
        [Required]
        public required string UrlDocumentacion { get; set; }
        [Required]
        public required int CveProducto { get; set; }
    }
}