using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubastaApi.Data;
using SubastaApi.DTOs;
using SubastaApi.Entidades;

namespace SubastaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubastaController : ControllerBase
    {
        private readonly SubastaDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SubastaController(SubastaDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET api/subasta
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subasta>>> Get()
        {
            var subastas = await _context.Subastas
                .Include(s => s.ProductoRef)
                .Include(s => s.TipoSubastaRef)
                .Include(s => s.StatusSubastaRef)
                .Include(s => s.UsuarioGanadorRef)
                .ToListAsync();

            return Ok(subastas);
        }

        // GET api/subasta/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Subasta>> Get(int id)
        {
            var subasta = await _context.Subastas
                .Include(s => s.ProductoRef)
                    .ThenInclude(p => p!.Fotos)
                .Include(s => s.TipoSubastaRef)
                .Include(s => s.StatusSubastaRef)
                .Include(s => s.UsuarioGanadorRef)
                .Include(s => s.Ofertas.OrderByDescending(o => o.Monto))
                .FirstOrDefaultAsync(s => s.IdSubasta == id);

            if (subasta is null)
                return NotFound();

            // Si es subasta sellada no se exponen las ofertas hasta que termine
            if (subasta.CveTipoSubasta == 3 && subasta.CveStatusSubasta == 2)
                subasta.Ofertas = [];

            return Ok(subasta);
        }

        // GET api/subasta/activas
        [HttpGet("activas")]
        public async Task<ActionResult<IEnumerable<Subasta>>> GetActivas()
        {
            var subastas = await _context.Subastas
                .Include(s => s.ProductoRef)
                    .ThenInclude(p => p!.Fotos)
                .Include(s => s.TipoSubastaRef)
                .Include(s => s.StatusSubastaRef)
                .Where(s => s.CveStatusSubasta == 2)
                .ToListAsync();

            return Ok(subastas);
        }

        // GET api/subasta/vendedor/1
        [HttpGet("vendedor/{idUsuario:int}")]
        public async Task<ActionResult<IEnumerable<Subasta>>> GetByVendedor(int idUsuario)
        {
            var subastas = await _context.Subastas
                .Include(s => s.ProductoRef)
                .Include(s => s.TipoSubastaRef)
                .Include(s => s.StatusSubastaRef)
                .Where(s => s.ProductoRef!.CveUsuario == idUsuario)
                .ToListAsync();

            return Ok(subastas);
        }

        // POST api/subasta/crear
        [HttpPost("crear")]
        public async Task<ActionResult> Crear([FromForm] CrearSubastaDto dto, IWebHostEnvironment env)
        {
            // Verificar que el usuario existe
            var usuario = await _context.Usuarios.FindAsync(dto.CveUsuario);
            if (usuario is null)
                return NotFound("Usuario no encontrado");

            // 1. Crear el producto
            var producto = new Producto
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Ubicacion = dto.Ubicacion,
                CveCategoria = dto.CveCategoria,
                CveCondicion = dto.CveCondicion,
                CveUsuario = dto.CveUsuario,
                CveStatusProducto = 2 // En Subasta directo
            };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            // 2. Guardar fotos si vienen
            if (dto.Fotos != null && dto.Fotos.Count > 0)
            {
                var extensionesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var carpeta = Path.Combine(env.WebRootPath, "fotos", producto.IdProducto.ToString());

                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                foreach (var foto in dto.Fotos)
                {
                    var extension = Path.GetExtension(foto.FileName).ToLower();

                    if (!extensionesPermitidas.Contains(extension))
                        continue; // Omite archivos no válidos

                    if (foto.Length > 5 * 1024 * 1024)
                        continue; // Omite archivos mayores a 5MB

                    var nombreArchivo = $"{Guid.NewGuid()}{extension}";
                    var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

                    using var stream = new FileStream(rutaCompleta, FileMode.Create);
                    await foto.CopyToAsync(stream);

                    _context.FotosProducto.Add(new FotoProducto
                    {
                        Url = $"/fotos/{producto.IdProducto}/{nombreArchivo}",
                        CveProducto = producto.IdProducto
                    });
                }

                await _context.SaveChangesAsync();
            }

            // 3. Guardar datos de vehículo si aplica
            if (dto.CveCategoria == 1 && dto.Marca != null)
            {
                _context.Vehiculos.Add(new Vehiculo
                {
                    Marca = dto.Marca,
                    Modelo = dto.Modelo!,
                    Anio = dto.Anio ?? 0,
                    Kilometraje = dto.Kilometraje ?? 0,
                    NumeroSerie = dto.NumeroSerie ?? 0,
                    UrlDocumentacion = dto.UrlDocumentacionVehiculo ?? "",
                    CveProducto = producto.IdProducto
                });

                await _context.SaveChangesAsync();
            }

            // 4. Guardar datos de inmueble si aplica
            if (dto.CveCategoria == 2 && dto.SuperficieTerreno != null)
            {
                _context.Inmuebles.Add(new Inmueble
                {
                    SuperficieTerreno = (int)(dto.SuperficieTerreno ?? 0),
                    SuperficieConstruida = (int)(dto.SuperficieConstruida ?? 0),
                    NumeroHabitaciones = dto.NumeroHabitaciones ?? 0,
                    UrlDocumentacion = dto.UrlDocumentacionInmueble ?? "",
                    CveProducto = producto.IdProducto
                });

                await _context.SaveChangesAsync();
            }

            // 5. Crear la subasta
            var subasta = new Subasta
            {
                PrecioInicial = dto.PrecioInicial,
                PrecioActual = dto.CveTipoSubasta == 2
                    ? dto.PrecioInicial          // holandesa arranca en el precio inicial
                    : dto.PrecioInicial,         // inglesa y sellada igual
                Incremento = dto.Incremento,
                FechaInicio = dto.FechaInicio,
                FechaFinal = dto.FechaFinal,
                CveTipoSubasta = dto.CveTipoSubasta,
                CveProducto = producto.IdProducto,
                CveStatusSubasta = 1,            // Pendiente
                CveUsuarioGanador = null
            };

            // Validaciones por tipo
            if (dto.CveTipoSubasta == 1 && dto.Incremento == null)
                return BadRequest("La subasta inglesa requiere un incremento mínimo");

            if (dto.CveTipoSubasta == 2 && dto.PrecioMinimo == null)
                return BadRequest("La subasta holandesa requiere un precio mínimo");

            if (dto.CveTipoSubasta == 2)
                subasta.PrecioActual = dto.PrecioMinimo!.Value;

            _context.Subastas.Add(subasta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = subasta.IdSubasta }, new
            {
                subasta.IdSubasta,
                producto.IdProducto
            });
        }

        // PUT api/subasta/1 — editar antes de iniciar
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, Subasta subasta)
        {
            if (id != subasta.IdSubasta)
                return BadRequest("Los ids deben coincidir");

            var subastaDb = await _context.Subastas.FindAsync(id);

            if (subastaDb is null)
                return NotFound();

            // Solo se puede editar si está Pendiente
            if (subastaDb.CveStatusSubasta != 1)
                return BadRequest("Solo se puede editar una subasta con status Pendiente");

            _context.Update(subasta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT api/subasta/1/cancelar
        [HttpPut("{id:int}/cancelar")]
        public async Task<ActionResult> Cancelar(int id)
        {
            var subasta = await _context.Subastas
                .Include(s => s.ProductoRef)
                .FirstOrDefaultAsync(s => s.IdSubasta == id);

            if (subasta is null)
                return NotFound();

            // Solo se puede cancelar si está Pendiente
            if (subasta.CveStatusSubasta != 1)
                return BadRequest("Solo se puede cancelar una subasta con status Pendiente");

            // Validar que no hayan pasado más de 30 minutos desde la creación
            var minutosTranscurridos = (DateTime.UtcNow - subasta.FechaInicio).TotalMinutes;

            if (minutosTranscurridos > 30)
                return BadRequest("No se puede cancelar la subasta, han pasado más de 30 minutos");

            // Cancelar subasta
            subasta.CveStatusSubasta = 4;

            // Regresar el producto a Disponible
            if (subasta.ProductoRef != null)
                subasta.ProductoRef.CveStatusProducto = 1;

            _context.Update(subasta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT api/subasta/1/extender
        [HttpPut("{id:int}/extender")]
        public async Task<ActionResult> Extender(int id, [FromBody] int minutosExtra)
        {
            var subasta = await _context.Subastas.FindAsync(id);

            if (subasta is null)
                return NotFound();

            // Solo se puede extender si está Activa
            if (subasta.CveStatusSubasta != 2)
                return BadRequest("Solo se puede extender una subasta Activa");

            if (minutosExtra <= 0)
                return BadRequest("Los minutos de extensión deben ser mayor a 0");

            subasta.FechaFinal = subasta.FechaFinal.AddMinutes(minutosExtra);

            _context.Update(subasta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT api/subasta/1/finalizar
        [HttpPut("{id:int}/finalizar")]
        public async Task<ActionResult> Finalizar(int id)
        {
            var subasta = await _context.Subastas
                .Include(s => s.Ofertas)
                .Include(s => s.ProductoRef)
                .FirstOrDefaultAsync(s => s.IdSubasta == id);

            if (subasta is null)
                return NotFound();

            if (subasta.CveStatusSubasta != 2)
                return BadRequest("Solo se puede finalizar una subasta Activa");

            // Determinar ganador según tipo de subasta
            Oferta? ofertaGanadora = null;

            // Inglesa (1) y Sellada (3): gana la oferta más alta
            if (subasta.CveTipoSubasta == 1 || subasta.CveTipoSubasta == 3)
                ofertaGanadora = subasta.Ofertas.OrderByDescending(o => o.Monto).FirstOrDefault();

            // Holandesa (2): gana la primera oferta que aceptó el precio
            if (subasta.CveTipoSubasta == 2)
                ofertaGanadora = subasta.Ofertas.OrderBy(o => o.Fecha).FirstOrDefault();

            if (ofertaGanadora != null)
            {
                subasta.CveUsuarioGanador = ofertaGanadora.CveUsuario;
                subasta.CveStatusSubasta = 3;
                subasta.ProductoRef!.CveStatusProducto = 3; // Vendido

                // Calcular fecha límite de pago según tipo de producto
                int horasPago = 48; // Artículo general por defecto

                bool esVehiculo = await _context.Vehiculos
                    .AnyAsync(v => v.CveProducto == subasta.CveProducto);

                bool esInmueble = await _context.Inmuebles
                    .AnyAsync(i => i.CveProducto == subasta.CveProducto);

                if (esVehiculo) horasPago = 72;
                if (esInmueble) horasPago = 168; // 7 días

                // Crear el pago
                var pago = new Pago
                {
                    Monto = ofertaGanadora.Monto,
                    FechaRealizacion = DateTime.UtcNow,
                    FechaLimite = DateTime.UtcNow.AddHours(horasPago),
                    CveStatusPago = 1, // Pendiente
                    CveSubasta = subasta.IdSubasta
                };

                _context.Pagos.Add(pago);

                // Notificar al ganador
                var notificacionGanador = new Notificacion
                {
                    Descripcion = $"¡Felicidades! Ganaste la subasta. Tienes {horasPago} horas para realizar el pago.",
                    FechaEnvio = DateTime.UtcNow,
                    Leida = false,
                    CveUsuario = ofertaGanadora.CveUsuario,
                    CveTipoNotificacion = 3, // Pago Pendiente
                    CveSubasta = subasta.IdSubasta
                };

                _context.Notificaciones.Add(notificacionGanador);
            }
            else
            {
                // Sin ofertas, la subasta finaliza sin ganador
                subasta.CveStatusSubasta = 3;
                subasta.ProductoRef!.CveStatusProducto = 1; // Regresa a Disponible
            }

            _context.Update(subasta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT api/subasta/1/iniciar
        [HttpPut("{id:int}/iniciar")]
        public async Task<ActionResult> Iniciar(int id)
        {
            var subasta = await _context.Subastas.FindAsync(id);

            if (subasta is null)
                return NotFound();

            if (subasta.CveStatusSubasta != 1)
                return BadRequest("Solo se puede iniciar una subasta con status Pendiente");

            if (DateTime.UtcNow < subasta.FechaInicio)
                return BadRequest($"La subasta no puede iniciar antes de su fecha programada: {subasta.FechaInicio}");

            subasta.CveStatusSubasta = 2; // Activa
            _context.Update(subasta);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}