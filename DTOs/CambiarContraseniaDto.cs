namespace SubastaApi.DTOs
{
    public class CambiarContraseniaDto
    {
        public required string ContraseniaActual { get; set; }
        public required string ContraseniaNueva { get; set; }
    }
}