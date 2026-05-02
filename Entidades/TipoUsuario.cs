using System.ComponentModel.DataAnnotations;

namespace SubastaApi.Entidades
{
    public class TipoUsuario
    {
        public int IdTipoUsuario { get; set; }
        [Required]
        public required string Descripcion { get; set; }
    }
}