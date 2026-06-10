using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubastaApi.Data;
using SubastaApi.Entidades;

namespace SubastaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoUsuarioController : ControllerBase
    {
        private readonly SubastaDbContext _context;

        public TipoUsuarioController(SubastaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoUsuario>>> Get()
        {
            return await _context.TiposUsuario.ToListAsync();
        }
    }
}