using System.ComponentModel.DataAnnotations;

namespace SubastaApi.Entidades
{
    public class Inmueble
    {
        public int IdInmueble { get; set; }
        public int SuperficieTerreno { get; set; }
        public int SuperficieConstruida { get; set; }
        public int NumeroHabitaciones { get; set; }
        [Required]
        public required string UrlDocumentacion { get; set; }
        public int CveProducto { get; set; }
        
        public Producto? ProductoRef { get; set; }
    }
}