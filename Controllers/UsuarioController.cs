using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubastaApi.Entidades;
using SubastaApi.Data;

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
    }
}