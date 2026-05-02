using System.ComponentModel.DataAnnotations;

namespace SubastaApi.Entidades
{
    public class TIpoNotificaion
    {
        public int IdIdentificacion { get; set; }
        [Required]
        public required string Descripcion { get; set; }
    }
}