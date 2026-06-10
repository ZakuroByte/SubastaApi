using System.ComponentModel.DataAnnotations;

namespace SubastaApi.Entidades
{
    public class FotoProducto
    {
        public int IdFoto { get; set; }
        [Required]
        public required string Url { get; set; }
        public int CveProducto { get; set; }
        
        public Producto? ProductoRef { get; set; }
    }
}