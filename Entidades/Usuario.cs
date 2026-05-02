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
        [Required]
        public required int CveTipoUsuario { get; set; }

        public TipoUsuario? TipoUsuario { get; set; }
        
    }
}