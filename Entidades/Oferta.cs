using System.ComponentModel.DataAnnotations;

namespace SubastaApi.Entidades
{
    public class Oferta
    {
        public int IdOferta { get; set; }
        public  DateTime Fecha { get; set; }
        public float Monto { get; set; }
        public int CveUsuario { get; set; }
        public int CveSubasta { get; set; }

        public Usuario? UsuarioRef { get; set; }
        public Subasta? SubastaRef { get; set; }
        public ICollection<Notificacion> Notificaciones { get; set; } = [];
    }
}