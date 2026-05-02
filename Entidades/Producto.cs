using System.ComponentModel.DataAnnotations;

namespace SubastaApi.Entidades
{
    public class Producto
    {
        public int IdProducto { get; set; }
        [Required]
        public required string Nombre { get; set; }
        [Required]
        public required string Descripcion { get; set; }
        [Required]
        public required string Ubicacion { get; set; }
        [Required]
        public required int CveCategoria { get; set; }
        [Required]
        public required int CveCondicion { get; set; }
        [Required]
        public required int CveUsuario { get; set; }
        [Required]
        public required int CveStatus { get; set; }
    }
}