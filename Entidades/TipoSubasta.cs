using System.ComponentModel.DataAnnotations;

namespace SubastaApi.Entidades

{
    public class TipoSubasta
    {
        public int IdTipoSubasta { get; set; }
        [Required]
        public required string Descripcion { get; set; }

        public ICollection<Subasta> Subastas { get; set; } = [];
    }
}