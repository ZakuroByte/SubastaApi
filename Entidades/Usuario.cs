using System.ComponentModel.DataAnnotations;

namespace SubastaApi.Entidades
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        [EmailAddress]
        [Required]
        public required string Correo { get; set; }
        [Required]
        public required string Contrasenia { get; set; }
        [Required]
        public required string Nombre { get; set; }
        [Required]
        public required string ApellidoPaterno { get; set; }
        [Required]
        public required string ApellidoMaterno { get; set; }
        public int? Calificacion { get; set; }
        public int CveTipoUsuario { get; set; }

        public TipoUsuario? TipoUsuarioRef { get; set; }
        public ICollection<Producto> Productos { get; set; } = [];
        public ICollection<Oferta> Ofertas { get; set; } = [];
        public ICollection<Subasta> Subastas { get; set; } = [];
        public ICollection<Notificacion> Notificaciones { get; set; } = [];
        public ICollection<Calificacion> CalificacionesRecibidas { get; set; } = [];
        public ICollection<Calificacion> CalificacionesDadas { get; set; } = [];
    }
}