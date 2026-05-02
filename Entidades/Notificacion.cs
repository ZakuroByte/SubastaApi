using System.ComponentModel.DataAnnotations;

namespace SubastaApi.Entidades
{
    public class Notificacion
    {
        public int IdNotificacion { get; set; }
        [Required]
        public required string Descripcion { get; set; }
        [Required]
        public required DateTime FechaEnvio { get; set; }
        [Required]
        public required bool Leida { get; set; }
        [Required]
        public required int CveUsuario { get; set; }
        [Required]
        public required int CveTipoNotificacion { get; set; }
        [Required]
        public required int CveOferta { get; set; }
        [Required]
        public required int CveSubasta { get; set; }
    }
}