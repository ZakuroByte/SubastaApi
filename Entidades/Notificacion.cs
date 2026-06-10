using System.ComponentModel.DataAnnotations;

namespace SubastaApi.Entidades
{
    public class Notificacion
    {
        public int IdNotificacion { get; set; }
        [Required]
        public required string Descripcion { get; set; }
        public DateTime FechaEnvio { get; set; }
        public bool Leida { get; set; }
        public int CveUsuario { get; set; }
        public int CveTipoNotificacion { get; set; }
        public int? CveOferta { get; set; }
        public int? CveSubasta { get; set; }

        public Usuario? UsuarioRef { get; set; }
        public TipoNotificacion? TipoNotificacionRef { get; set; }
        public Oferta? OfertaRef { get; set; }
        public Subasta? SubastaRef { get; set; }
    }
}