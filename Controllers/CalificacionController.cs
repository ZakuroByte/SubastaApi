using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubastaApi.Data;
using SubastaApi.Entidades;

namespace SubastaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalificacionController : ControllerBase
    {
        private readonly SubastaDbContext _context;

        public CalificacionController(SubastaDbContext context)
        {
            _context = context;
        }

        // GET api/calificacion/usuario/1
        [HttpGet("usuario/{idUsuario:int}")]
        public async Task<ActionResult<IEnumerable<Calificacion>>> GetByUsuario(int idUsuario)
        {
            var calificaciones = await _context.Calificaciones
                .Include(c => c.UsuarioCalificador)
                .Where(c => c.CveUsuarioCalificado == idUsuario)
                .OrderByDescending(c => c.Fecha)
                .ToListAsync();

            return Ok(calificaciones);
        }

        // GET api/calificacion/promedio/1
        [HttpGet("promedio/{idUsuario:int}")]
        public async Task<ActionResult<double>> GetPromedio(int idUsuario)
        {
            var tieneCalificaciones = await _context.Calificaciones
                .AnyAsync(c => c.CveUsuarioCalificado == idUsuario);

            if (!tieneCalificaciones)
                return Ok(0);

            var promedio = await _context.Calificaciones
                .Where(c => c.CveUsuarioCalificado == idUsuario)
                .AverageAsync(c => c.Estrellas);

            return Ok(Math.Round(promedio, 1));
        }

        // POST api/calificacion
        [HttpPost]
        public async Task<ActionResult> Post(Calificacion calificacion)
        {
            // Verificar que la subasta existe y está finalizada
            var subasta = await _context.Subastas.FindAsync(calificacion.CveSubasta);

            if (subasta is null)
                return NotFound("Subasta no encontrada");

            if (subasta.CveStatusSubasta != 3)
                return BadRequest("Solo se puede calificar en subastas finalizadas");

            // Verificar que el pago fue realizado
            var pago = await _context.Pagos
                .FirstOrDefaultAsync(p => p.CveSubasta == calificacion.CveSubasta);

            if (pago is null || pago.CveStatusPago != 2)
                return BadRequest("Solo se puede calificar después de que el pago fue realizado");

            // Verificar que el calificador participó en la subasta
            bool esGanador = subasta.CveUsuarioGanador == calificacion.CveUsuarioCalificador;
            bool esVendedor = await _context.Productos
                .AnyAsync(p => p.IdProducto == subasta.CveProducto &&
                               p.CveUsuario == calificacion.CveUsuarioCalificador);

            if (!esGanador && !esVendedor)
                return BadRequest("Solo el comprador y el vendedor pueden calificar");

            // Verificar que no haya calificado antes en esta subasta
            bool yaCalifico = await _context.Calificaciones
                .AnyAsync(c => c.CveSubasta == calificacion.CveSubasta &&
                               c.CveUsuarioCalificador == calificacion.CveUsuarioCalificador);

            if (yaCalifico)
                return BadRequest("Ya calificaste en esta subasta");

            // Verificar que no se califique a sí mismo
            if (calificacion.CveUsuarioCalificado == calificacion.CveUsuarioCalificador)
                return BadRequest("No puedes calificarte a ti mismo");

            calificacion.Fecha = DateTime.UtcNow;

            _context.Calificaciones.Add(calificacion);
            await _context.SaveChangesAsync();

            // Actualizar el promedio de calificacion en el usuario
            var promedio = await _context.Calificaciones
                .Where(c => c.CveUsuarioCalificado == calificacion.CveUsuarioCalificado)
                .AverageAsync(c => c.Estrellas);

            var usuario = await _context.Usuarios.FindAsync(calificacion.CveUsuarioCalificado);

            if (usuario != null)
            {
                usuario.Calificacion = (int)Math.Round(promedio);
                _context.Update(usuario);
                await _context.SaveChangesAsync();
            }

            return Ok("Calificación registrada correctamente");
        }

        // PUT api/calificacion/1
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, Calificacion calificacion)
        {
            if (id != calificacion.IdCalificacion)
                return BadRequest("Los ids deben coincidir");

            var calificacionDb = await _context.Calificaciones
                .Include(c => c.SubastaRef)
                .FirstOrDefaultAsync(c => c.IdCalificacion == id);

            if (calificacionDb is null)
                return NotFound();

            // Solo se puede editar si la subasta finalizó hace menos de 7 días
            var diasTranscurridos = (DateTime.UtcNow - calificacionDb.Fecha).TotalDays;

            if (diasTranscurridos > 7)
                return BadRequest("Solo se puede editar una calificación dentro de los primeros 7 días");

            calificacionDb.Estrellas = calificacion.Estrellas;
            calificacionDb.Comentario = calificacion.Comentario;
            calificacionDb.Fecha = DateTime.UtcNow;

            _context.Update(calificacionDb);
            await _context.SaveChangesAsync();

            // Recalcular promedio del usuario
            var promedio = await _context.Calificaciones
                .Where(c => c.CveUsuarioCalificado == calificacionDb.CveUsuarioCalificado)
                .AverageAsync(c => c.Estrellas);

            var usuario = await _context.Usuarios.FindAsync(calificacionDb.CveUsuarioCalificado);

            if (usuario != null)
            {
                usuario.Calificacion = (int)Math.Round(promedio);
                _context.Update(usuario);
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }
    }
}