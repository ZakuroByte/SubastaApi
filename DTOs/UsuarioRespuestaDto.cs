namespace SubastaApi.DTOs
{
    public class UsuarioRespuestaDto
    {
        public int IdUsuario { get; set; }
        public string Correo { get; set; } = "";
        public string Nombre { get; set; } = "";
        public string ApellidoPaterno { get; set; } = "";
        public string ApellidoMaterno { get; set; } = "";
        public int? Calificacion { get; set; }
        public int CveTipoUsuario { get; set; }
        public string TipoUsuario { get; set; } = "";
    }
}