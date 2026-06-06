using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubastaApi.Data;
using SubastaApi.Entidades;

namespace SubastaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagoController : ControllerBase
    {
        private readonly SubastaDbContext _context;

        public PagoController(SubastaDbContext context)
        {
            _context = context;
        }

        // GET api/pago/subasta/1
        [HttpGet("subasta/{idSubasta:int}")]
        public async Task<ActionResult<Pago>> GetBySubasta(int idSubasta)
        {
            var pago = await _context.Pagos
                .Include(p => p.StatusPagoRef)
                .Include(p => p.SubastaRef)
                .FirstOrDefaultAsync(p => p.CveSubasta == idSubasta);

            if (pago is null)
                return NotFound();

            return Ok(pago);
        }

        // GET api/pago/usuario/1
        [HttpGet("usuario/{idUsuario:int}")]
        public async Task<ActionResult<IEnumerable<Pago>>> GetByUsuario(int idUsuario)
        {
            var pagos = await _context.Pagos
                .Include(p => p.StatusPagoRef)
                .Include(p => p.SubastaRef)
                    .ThenInclude(s => s!.ProductoRef)
                .Where(p => p.SubastaRef!.CveUsuarioGanador == idUsuario)
                .OrderByDescending(p => p.FechaLimite)
                .ToListAsync();

            return Ok(pagos);
        }

        // PUT api/pago/1/pagar
        [HttpPut("{id:int}/pagar")]
        public async Task<ActionResult> Pagar(int id)
        {
            var pago = await _context.Pagos
                .Include(p => p.SubastaRef)
                    .ThenInclude(s => s!.ProductoRef)
                .FirstOrDefaultAsync(p => p.IdPago == id);

            if (pago is null)
                return NotFound();

            // Verificar que el pago esté pendiente
            if (pago.CveStatusPago != 1)
                return BadRequest("Este pago no está pendiente");

            // Verificar que no haya vencido
            if (DateTime.UtcNow > pago.FechaLimite)
                return BadRequest("El tiempo límite de pago ha vencido");

            pago.CveStatusPago = 2; // Pagado
            pago.FechaRealizacion = DateTime.UtcNow;

            _context.Update(pago);

            // Notificar al vendedor que recibió el pago
            _context.Notificaciones.Add(new Notificacion
            {
                Descripcion = $"El comprador ha realizado el pago de {pago.Monto}.",
                FechaEnvio = DateTime.UtcNow,
                Leida = false,
                CveUsuario = pago.SubastaRef!.ProductoRef!.CveUsuario,
                CveTipoNotificacion = 3,
                CveSubasta = pago.CveSubasta
            });

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}