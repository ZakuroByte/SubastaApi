using System.ComponentModel.DataAnnotations;

namespace SubastaApi.Entidades
{
    public class Subasta
    {
        public int IdSubasta { get; set; }
        public decimal PrecioInicial { get; set; }
        public decimal PrecioActual { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public int Cantidad { get; set; }
        public int CveTipoSubasta { get; set; }
        public int CveProducto { get; set; }
        public int CveStatusSubasta { get; set; }
        public int? CveUsuarioGanador { get; set; }
        public decimal Decremento { get; set; } = 5;      // porcentaje fijo 5%
        public int IntervaloMinutos { get; set; } = 10;   // cada 10 minutos
        public decimal? Incremento { get; set; }
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