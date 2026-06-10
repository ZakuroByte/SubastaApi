using System.ComponentModel.DataAnnotations;

namespace SubastaApi.Entidades
{
    public class Pago
    {
        public int IdPago { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaLimite { get; set; }
        public DateTime? FechaRealizacion { get; set; }
        public int CveSubasta { get; set; }
        public int CveStatusPago { get; set; }

        public Subasta? SubastaRef { get; set; }
        public StatusPago? StatusPagoRef { get; set; }
    }
}