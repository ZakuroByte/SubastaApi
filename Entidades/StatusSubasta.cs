using System.ComponentModel.DataAnnotations;

namespace SubastaApi.Entidades
{
    public class StatusSubasta
    {
        public int IdStatusSubasta { get; set; }
        [Required]
        public required string Descripcion { get; set; }

        public ICollection<Subasta> Subastas { get; set; } = [];
    }
}