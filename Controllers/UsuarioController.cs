using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubastaApi.Entidades;
using SubastaApi.Data;
using SubastaApi.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace SubastaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly SubastaDbContext _context;

        public UsuarioController(SubastaDbContext context)
        {
            _context = context;
        }

        // GET api/usuario/1
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Usuario>> Get(int id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.TipoUsuarioRef)
                .Include(u => u.CalificacionesRecibidas)
                .Include(u => u.Productos)
                .FirstOrDefaultAsync(x => x.IdUsuario == id);

            if (usuario is null)
                return NotFound();

            // Nunca regresar la contraseña
            usuario.Contrasenia = "";

            return Ok(usuario);
        }

        // PUT api/usuario/1
        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] EditarUsuarioDto dto)
        {
            if (id != dto.IdUsuario)
                return BadRequest("Los ids deben coincidir");

            var usuarioDb = await _context.Usuarios.FindAsync(id);

            if (usuarioDb is null)
                return NotFound();

            // Verificar si el correo ya está en uso por otro usuario
            bool correoEnUso = await _context.Usuarios
                .AnyAsync(u => u.Correo == dto.Correo && u.IdUsuario != id);

            if (correoEnUso)
                return BadRequest("El correo ya está en uso");

            usuarioDb.Nombre = dto.Nombre;
            usuarioDb.ApellidoPaterno = dto.ApellidoPaterno;
            usuarioDb.ApellidoMaterno = dto.ApellidoMaterno;
            usuarioDb.Correo = dto.Correo;

            _context.Update(usuarioDb);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT api/usuario/1/contrasenia
        [Authorize]
        [HttpPut("{id:int}/contrasenia")]
        public async Task<ActionResult> CambiarContrasenia(int id, [FromBody] CambiarContraseniaDto dto)
        {
            var usuarioDb = await _context.Usuarios.FindAsync(id);

            if (usuarioDb is null)
                return NotFound();

            // Verificar contraseña actual
            bool contraseniaValida = BCrypt.Net.BCrypt.Verify(dto.ContraseniaActual, usuarioDb.Contrasenia);

            if (!contraseniaValida)
                return BadRequest("La contraseña actual es incorrecta");

            if (dto.ContraseniaNueva.Length < 8)
                return BadRequest("La contraseña debe tener al menos 8 caracteres");

            usuarioDb.Contrasenia = BCrypt.Net.BCrypt.HashPassword(dto.ContraseniaNueva);

            _context.Update(usuarioDb);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}