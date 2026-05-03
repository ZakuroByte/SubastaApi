using System.ComponentModel.DataAnnotations;

namespace SubastaApi.Entidades
{
    public class Subasta
    {
        public int IdSubasta { get; set; }
        public float PrecioInicial { get; set; }
        public float PrecioActual { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public int Cantidad { get; set; }
        public int CveTipoSubasta { get; set; }
        public int CveProducto { get; set; }
        public int CveStatusSubasta { get; set; }
        public int? CveUsuarioGanador { get; set; }

        public TipoSubasta? TipoSubastaRef { get; set; }
        public Producto? ProductoRef { get; set; }
        public StatusSubasta? StatusSubastaRef { get; set; }
        public Usuario? UsuarioGanadorRef { get; set; }
        public Pago? PagoRef { get; set; }
        public Calificacion? CalificacionRef { get; set; }
        public ICollection<Oferta> Ofertas { get; set; } = [];
        public ICollection<Notificacion> Notificaciones { get; set; } = [];
        
    }
}