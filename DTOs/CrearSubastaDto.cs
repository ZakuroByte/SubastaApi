namespace SubastaApi.DTOs
{
    public class CrearSubastaDto
    {
        // Datos del producto
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; }
        public required string Ubicacion { get; set; }
        public int CveCategoria { get; set; }
        public int CveCondicion { get; set; }
        public int CveUsuario { get; set; }

        // Fotos del producto
        public List<IFormFile>? Fotos { get; set; }

        // Datos de vehículo (opcional)
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public int? Anio { get; set; }
        public float? Kilometraje { get; set; }
        public int? NumeroSerie { get; set; }
        public string? UrlDocumentacionVehiculo { get; set; }

        // Datos de inmueble (opcional)
        public float? SuperficieTerreno { get; set; }
        public float? SuperficieConstruida { get; set; } 
        public int? NumeroHabitaciones { get; set; }  
        public string? UrlDocumentacionInmueble { get; set; }

        // Datos de la subasta
        public decimal PrecioInicial { get; set; }
        public decimal? PrecioMinimo { get; set; }    // solo holandesa
        public decimal? Incremento { get; set; }      // solo inglesa
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public int CveTipoSubasta { get; set; }
    }
}