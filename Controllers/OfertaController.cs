using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubastaApi.Data;
using SubastaApi.Entidades;

namespace SubastaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OfertaController : ControllerBase
    {
        private readonly SubastaDbContext _context;

        public OfertaController(SubastaDbContext context)
        {
            _context = context;
        }

        // GET api/oferta/subasta/1
        [HttpGet("subasta/{idSubasta:int}")]
        public async Task<ActionResult<IEnumerable<Oferta>>> GetBySubasta(int idSubasta)
        {
            var subasta = await _context.Subastas.FindAsync(idSubasta);

            if (subasta is null)
                return NotFound("Subasta no encontrada");

            // Si es sellada y está activa no se muestran las ofertas
            if (subasta.CveTipoSubasta == 3 && subasta.CveStatusSubasta == 2)
                return BadRequest("Las ofertas de una subasta sellada no son visibles hasta que finalice");

            var ofertas = await _context.Ofertas
                .Include(o => o.UsuarioRef)
                .Where(o => o.CveSubasta == idSubasta)
                .OrderByDescending(o => o.Monto)
                .ToListAsync();

            return Ok(ofertas);
        }

        // GET api/oferta/usuario/1
        [HttpGet("usuario/{idUsuario:int}")]
        public async Task<ActionResult<IEnumerable<Oferta>>> GetByUsuario(int idUsuario)
        {
            var ofertas = await _context.Ofertas
                .Include(o => o.SubastaRef)
                .Where(o => o.CveUsuario == idUsuario)
                .OrderByDescending(o => o.Fecha)
                .ToListAsync();

            return Ok(ofertas);
        }

        // POST api/oferta — Inglesa y Sellada
        [HttpPost]
        public async Task<ActionResult> Post(Oferta oferta)
        {
            var subasta = await _context.Subastas
                .Include(s => s.Ofertas)
                .Include(s => s.ProductoRef)
                .FirstOrDefaultAsync(s => s.IdSubasta == oferta.CveSubasta);

            if (subasta is null)
                return NotFound("Subasta no encontrada");

            // Verificar que la subasta esté activa
            if (subasta.CveStatusSubasta != 2)
                return BadRequest("La subasta no está activa");

            // Verificar que no haya terminado el tiempo
            if (DateTime.UtcNow >= subasta.FechaFinal)
                return BadRequest("El tiempo de la subasta ha terminado");

            // No permitir ofertar en subasta holandesa desde este endpoint
            if (subasta.CveTipoSubasta == 2)
                return BadRequest("Para subastas holandesas usa el endpoint api/oferta/aceptar");

            oferta.Fecha = DateTime.UtcNow;

            // Validaciones para subasta Inglesa
            if (subasta.CveTipoSubasta == 1)
            {
                // La oferta debe ser mayor al precio actual más el incremento
                var montoMinimo = (decimal)subasta.PrecioActual + (subasta.Incremento ?? 0);

                if ((decimal)oferta.Monto <= montoMinimo)
                    return BadRequest($"La oferta debe ser mayor a {montoMinimo}");

                // Actualizar precio actual de la subasta
                subasta.PrecioActual = oferta.Monto;

                // Notificar a los demás compradores que su oferta fue superada
                var ofertasSuperades = subasta.Ofertas
                    .Where(o => o.CveUsuario != oferta.CveUsuario)
                    .Select(o => o.CveUsuario)
                    .Distinct()
                    .ToList();

                foreach (var idUsuario in ofertasSuperades)
                {
                    _context.Notificaciones.Add(new Notificacion
                    {
                        Descripcion = $"Tu oferta fue superada. El precio actual es {subasta.PrecioActual}.",
                        FechaEnvio = DateTime.UtcNow,
                        Leida = false,
                        CveUsuario = idUsuario,
                        CveTipoNotificacion = 2, // Oferta Superada
                        CveSubasta = subasta.IdSubasta,
                        CveOferta = oferta.IdOferta
                    });
                }

                // Notificar al vendedor
                _context.Notificaciones.Add(new Notificacion
                {
                    Descripcion = $"Nueva oferta recibida por {oferta.Monto} en tu subasta.",
                    FechaEnvio = DateTime.UtcNow,
                    Leida = false,
                    CveUsuario = subasta.ProductoRef!.CveUsuario,
                    CveTipoNotificacion = 1, // Oferta Recibida
                    CveSubasta = subasta.IdSubasta
                });

                _context.Update(subasta);
            }

            // Validaciones para subasta Sellada
            if (subasta.CveTipoSubasta == 3)
            {
                // Verificar si el usuario ya tiene una oferta en esta subasta
                var ofertaExistente = await _context.Ofertas
                    .FirstOrDefaultAsync(o =>
                        o.CveSubasta == oferta.CveSubasta &&
                        o.CveUsuario == oferta.CveUsuario);

                if (ofertaExistente != null)
                    return BadRequest("Ya tienes una oferta en esta subasta, usa el endpoint de modificar");

                if (oferta.Monto <= 0)
                    return BadRequest("La oferta debe ser mayor a 0");
            }

            _context.Ofertas.Add(oferta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBySubasta), new { idSubasta = oferta.CveSubasta }, oferta);
        }

        // PUT api/oferta/1 — Solo para subasta Sellada
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, Oferta oferta)
        {
            if (id != oferta.IdOferta)
                return BadRequest("Los ids deben coincidir");

            var ofertaDb = await _context.Ofertas.FindAsync(id);

            if (ofertaDb is null)
                return NotFound();

            var subasta = await _context.Subastas.FindAsync(ofertaDb.CveSubasta);

            if (subasta is null)
                return NotFound("Subasta no encontrada");

            // Solo se puede modificar en subasta sellada
            if (subasta.CveTipoSubasta != 3)
                return BadRequest("Solo se pueden modificar ofertas en subastas selladas");

            // Solo si la subasta está activa
            if (subasta.CveStatusSubasta != 2)
                return BadRequest("La subasta no está activa");

            // Verificar que no haya terminado el tiempo
            if (DateTime.UtcNow >= subasta.FechaFinal)
                return BadRequest("El tiempo de la subasta ha terminado");

            if (oferta.Monto <= 0)
                return BadRequest("La oferta debe ser mayor a 0");

            ofertaDb.Monto = oferta.Monto;
            ofertaDb.Fecha = DateTime.UtcNow;

            _context.Update(ofertaDb);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST api/oferta/aceptar — Solo para subasta Holandesa
        [HttpPost("aceptar")]
        public async Task<ActionResult> AceptarPrecioHolandesa([FromBody] int idSubasta, [FromBody] int idUsuario)
        {
            var subasta = await _context.Subastas
                .Include(s => s.ProductoRef)
                .FirstOrDefaultAsync(s => s.IdSubasta == idSubasta);

            if (subasta is null)
                return NotFound("Subasta no encontrada");

            // Verificar que sea holandesa
            if (subasta.CveTipoSubasta != 2)
                return BadRequest("Este endpoint es solo para subastas holandesas");

            // Verificar que esté activa
            if (subasta.CveStatusSubasta != 2)
                return BadRequest("La subasta no está activa");

            // Verificar que no haya terminado
            if (DateTime.UtcNow >= subasta.FechaFinal)
                return BadRequest("El tiempo de la subasta ha terminado");

            // Crear la oferta con el precio actual
            var oferta = new Oferta
            {
                Monto = subasta.PrecioActual,
                Fecha = DateTime.UtcNow,
                CveUsuario = idUsuario,
                CveSubasta = idSubasta
            };

            _context.Ofertas.Add(oferta);

            // Finalizar subasta inmediatamente
            subasta.CveUsuarioGanador = idUsuario;
            subasta.CveStatusSubasta = 3; // Finalizada
            subasta.ProductoRef!.CveStatusProducto = 3; // Vendido

            // Calcular tiempo límite de pago
            int horasPago = 48;

            bool esVehiculo = await _context.Vehiculos
                .AnyAsync(v => v.CveProducto == subasta.CveProducto);

            bool esInmueble = await _context.Inmuebles
                .AnyAsync(i => i.CveProducto == subasta.CveProducto);

            if (esVehiculo) horasPago = 72;
            if (esInmueble) horasPago = 168;

            // Crear pago
            var pago = new Pago
            {
                Monto = subasta.PrecioActual,
                FechaRealizacion = DateTime.UtcNow,
                FechaLimite = DateTime.UtcNow.AddHours(horasPago),
                CveStatusPago = 1,
                CveSubasta = subasta.IdSubasta
            };

            _context.Pagos.Add(pago);

            // Notificar al ganador
            _context.Notificaciones.Add(new Notificacion
            {
                Descripcion = $"¡Compraste el producto! Tienes {horasPago} horas para realizar el pago.",
                FechaEnvio = DateTime.UtcNow,
                Leida = false,
                CveUsuario = idUsuario,
                CveTipoNotificacion = 3,
                CveSubasta = subasta.IdSubasta
            });

            // Notificar al vendedor
            _context.Notificaciones.Add(new Notificacion
            {
                Descripcion = $"¡Tu producto fue comprado por {subasta.PrecioActual}! El comprador tiene {horasPago} horas para pagar.",
                FechaEnvio = DateTime.UtcNow,
                Leida = false,
                CveUsuario = subasta.ProductoRef!.CveUsuario,
                CveTipoNotificacion = 1,
                CveSubasta = subasta.IdSubasta
            });

            _context.Update(subasta);
            await _context.SaveChangesAsync();

            return Ok("Compra realizada exitosamente");
        }
    }
}