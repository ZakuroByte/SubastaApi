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
        public int Anio { get; set; }
        public float Kilometraje { get; set; }
        public int NumeroSerie { get; set; }
        [Required]
        public required string UrlDocumentacion { get; set; }
        public int CveProducto { get; set; }

        public Producto? ProductoRef { get; set; }
    }
}