using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubastaApi.Data;
using SubastaApi.Entidades;

namespace SubastaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificacionController : ControllerBase
    {
        private readonly SubastaDbContext _context;

        public NotificacionController(SubastaDbContext context)
        {
            _context = context;
        }

        // GET api/notificacion/usuario/1
        [HttpGet("usuario/{idUsuario:int}")]
        public async Task<ActionResult<IEnumerable<Notificacion>>> GetByUsuario(int idUsuario)
        {
            var notificaciones = await _context.Notificaciones
                .Include(n => n.TipoNotificacionRef)
                .Where(n => n.CveUsuario == idUsuario)
                .OrderByDescending(n => n.FechaEnvio)
                .ToListAsync();

            return Ok(notificaciones);
        }

        // GET api/notificacion/usuario/1/noleidas
        [HttpGet("usuario/{idUsuario:int}/noleidas")]
        public async Task<ActionResult<int>> GetNoLeidas(int idUsuario)
        {
            var cantidad = await _context.Notificaciones
                .CountAsync(n => n.CveUsuario == idUsuario && !n.Leida);

            return Ok(cantidad);
        }

        // PUT api/notificacion/1/leer
        [HttpPut("{id:int}/leer")]
        public async Task<ActionResult> MarcarLeida(int id)
        {
            var notificacion = await _context.Notificaciones.FindAsync(id);

            if (notificacion is null)
                return NotFound();

            notificacion.Leida = true;
            _context.Update(notificacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT api/notificacion/usuario/1/leer
        [HttpPut("usuario/{idUsuario:int}/leer")]
        public async Task<ActionResult> MarcarTodasLeidas(int idUsuario)
        {
            var notificaciones = await _context.Notificaciones
                .Where(n => n.CveUsuario == idUsuario && !n.Leida)
                .ToListAsync();

            foreach (var notificacion in notificaciones)
                notificacion.Leida = true;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}