using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubastaApi.Entidades;
using SubastaApi.Data;
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

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Usuario>> Get(int id)
        {
            var autor = await _context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == id);
            
            if (autor is null)
            {
                return NotFound();
            }

            return autor;
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, Usuario usuario)
        {
            if(id != usuario.IdUsuario)
            {
                return BadRequest("Los ids deben coincidir");
            }

            _context.Update(usuario);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}