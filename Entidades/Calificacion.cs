using System.ComponentModel.DataAnnotations;

namespace SubastaApi.Entidades
{
    public class Calificacion
    {
        public int IdCalificacion { get; set; }
        [Range(0,5)]
        public int Estrellas { get; set; }
        public string? Comentario { get; set; }
        public DateTime Fecha { get; set; }
        public  int CveUsuarioCalificado { get; set; }
        public  int CveUsuarioCalificador { get; set; }
        public  int CveSubasta { get; set; }

        public Usuario? UsuarioCalificado { get; set; }
        public Usuario? UsuarioCalificador { get; set; }
        public Subasta? SubastaRef { get; set; }
    }
}