using System.ComponentModel.DataAnnotations;

namespace SubastaApi.Entidades
{
    public class TipoNotificacion
    {
        public int IdTipoNotificacion { get; set; }
        [Required]
        public required string Descripcion { get; set; }
        public ICollection<Notificacion> Notificaciones { get; set; } = [];
    }
}