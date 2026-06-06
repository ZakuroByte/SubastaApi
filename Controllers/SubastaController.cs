using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubastaApi.Data;
using SubastaApi.Entidades;

namespace SubastaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubastaController : ControllerBase
    {
        private readonly SubastaDbContext _context;

        public SubastaController(SubastaDbContext context)
        {
            _context = context;
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

        // POST api/subasta
        [HttpPost]
        public async Task<ActionResult> Post(Subasta subasta)
        {
            // Verificar que el producto existe y está disponible
            var producto = await _context.Productos.FindAsync(subasta.CveProducto);

            if (producto is null)
                return NotFound("Producto no encontrado");

            if (producto.CveStatusProducto != 1)
                return BadRequest("El producto no está disponible para subastarse");

            // Validaciones por tipo de subasta
            // Inglesa (1) y Sellada (3): necesitan precio inicial
            if (subasta.CveTipoSubasta == 1 || subasta.CveTipoSubasta == 3)
            {
                if (subasta.PrecioInicial <= 0)
                    return BadRequest("La subasta requiere un precio inicial válido");
            }

            // Holandesa (2): necesita precio inicial alto y precio actual como mínimo
            if (subasta.CveTipoSubasta == 2)
            {
                if (subasta.PrecioInicial <= 0)
                    return BadRequest("La subasta holandesa requiere un precio inicial válido");

                if (subasta.PrecioActual <= 0 || subasta.PrecioActual >= subasta.PrecioInicial)
                    return BadRequest("El precio mínimo debe ser mayor a 0 y menor al precio inicial");

                subasta.PrecioActual = subasta.PrecioInicial;
            }

            // Status inicial: Pendiente
            subasta.CveStatusSubasta = 1;
            subasta.CveUsuarioGanador = null;

            // En inglesa el precio actual arranca igual al inicial
            if (subasta.CveTipoSubasta == 1)
                subasta.PrecioActual = subasta.PrecioInicial;

            _context.Subastas.Add(subasta);

            // Cambiar status del producto a En Subasta
            producto.CveStatusProducto = 2;
            _context.Update(producto);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = subasta.IdSubasta }, subasta);
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
    }
}