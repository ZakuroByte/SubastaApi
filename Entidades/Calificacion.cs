using System.ComponentModel.DataAnnotations;

namespace SubastaApi.Entidades
{
    public class Calificacion
    {
        public int IdCalificacion { get; set; }
        [Required]
        [Range(0,5)]
        public required int Estrellas { get; set; }
        [Required]
        public required string Comentario { get; set; }
        [Required]
        public required DateTime Fecha { get; set; }
        [Required]
        public required int CveUsuarioCalificado { get; set; }
        [Required]
        public required int CveUsuarioCalificador { get; set; }
        [Required]
        public required int CveSubasta { get; set; }
    }
}