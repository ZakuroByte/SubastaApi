namespace SubastaApi.DTOs
{
    public class EditarUsuarioDto
    {
        public int IdUsuario { get; set; }
        public required string Nombre { get; set; }
        public required string ApellidoPaterno { get; set; }
        public required string ApellidoMaterno { get; set; }
        public required string Correo { get; set; }
    }
}